// Work Orders Page JavaScript
let authChecked = false
let dataFetched = false

document.addEventListener("DOMContentLoaded", () => {
  // Check authentication
  checkAuth().then((isAuth) => {
    authChecked = true
    if (!isAuth) {
      window.location.href = "index.html"
      return
    }
    fetchData().then(() => {
      dataFetched = true
      loadWorkOrders()
      setupEventListeners()
    })
  })
})

async function checkAuth() {
  // Placeholder for authentication check
  return true // Assume authenticated for now
}

async function fetchData() {
  // Placeholder for fetching data
  window.data = {
    PhieuCongViec: [],
    TaiSan: [],
    NhanVien: [],
  }
}

function getData(type) {
  return window.data[type]
}

function addData(type, data) {
  window.data[type].push(data)
}

function updateData(type, id, newData) {
  const index = window.data[type].findIndex((item) => item.MaPhieu === id)
  if (index !== -1) {
    window.data[type][index] = newData
  }
}

function deleteData(type, id) {
  window.data[type] = window.data[type].filter((item) => item.MaPhieu !== id)
}

function loadWorkOrders() {
  if (!dataFetched) return // Ensure data is fetched before rendering
  const workOrders = getData("PhieuCongViec") || []
  const assets = getData("TaiSan") || []
  const employees = getData("NhanVien") || []

  const tbody = document.getElementById("workOrdersTableBody")
  const searchInput = document.getElementById("searchInput")
  const statusFilter = document.getElementById("statusFilter")

  function renderWorkOrders() {
    const searchTerm = searchInput.value.toLowerCase()
    const statusValue = statusFilter.value

    const filtered = workOrders.filter((wo) => {
      const matchesSearch = wo.MoTa.toLowerCase().includes(searchTerm) || wo.MaPhieu.toLowerCase().includes(searchTerm)
      const matchesStatus = !statusValue || wo.TrangThai === statusValue
      return matchesSearch && matchesStatus
    })

    tbody.innerHTML = filtered
      .map((wo) => {
        const asset = assets.find((a) => a.MaTaiSan === wo.MaTaiSan)
        const employee = employees.find((e) => e.MaNhanVien === wo.MaNhanVienThucHien)

        const statusColors = {
          "Chờ xử lý": "bg-yellow-100 text-yellow-800",
          "Đang thực hiện": "bg-blue-100 text-blue-800",
          "Hoàn thành": "bg-green-100 text-green-800",
          "Đã hủy": "bg-red-100 text-red-800",
        }

        const priorityColors = {
          Thấp: "bg-gray-100 text-gray-800",
          "Trung bình": "bg-blue-100 text-blue-800",
          Cao: "bg-orange-100 text-orange-800",
          "Khẩn cấp": "bg-red-100 text-red-800",
        }

        return `
                <tr class="hover:bg-gray-50">
                    <td class="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">${wo.MaPhieu}</td>
                    <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">${asset ? asset.TenTaiSan : "N/A"}</td>
                    <td class="px-6 py-4 text-sm text-gray-900">${wo.MoTa}</td>
                    <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">${employee ? employee.HoTen : "N/A"}</td>
                    <td class="px-6 py-4 whitespace-nowrap">
                        <span class="px-2 py-1 text-xs font-semibold rounded-full ${priorityColors[wo.MucDoUuTien]}">${wo.MucDoUuTien}</span>
                    </td>
                    <td class="px-6 py-4 whitespace-nowrap">
                        <span class="px-2 py-1 text-xs font-semibold rounded-full ${statusColors[wo.TrangThai]}">${wo.TrangThai}</span>
                    </td>
                    <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">${formatCurrency(wo.ChiPhi)}</td>
                    <td class="px-6 py-4 whitespace-nowrap text-sm font-medium">
                        <button onclick="viewWorkOrder('${wo.MaPhieu}')" class="text-blue-600 hover:text-blue-900 mr-3">Xem</button>
                        <button onclick="editWorkOrder('${wo.MaPhieu}')" class="text-indigo-600 hover:text-indigo-900 mr-3">Sửa</button>
                        <button onclick="deleteWorkOrder('${wo.MaPhieu}')" class="text-red-600 hover:text-red-900">Xóa</button>
                    </td>
                </tr>
            `
      })
      .join("")

    if (filtered.length === 0) {
      tbody.innerHTML = '<tr><td colspan="8" class="px-6 py-4 text-center text-gray-500">Không có dữ liệu</td></tr>'
    }
  }

  searchInput.addEventListener("input", renderWorkOrders)
  statusFilter.addEventListener("change", renderWorkOrders)

  renderWorkOrders()
}

function setupEventListeners() {
  document.getElementById("addWorkOrderBtn").addEventListener("click", showAddModal)
}

function showAddModal() {
  const assets = getData("TaiSan") || []
  const employees = getData("NhanVien") || []

  const modal = document.getElementById("addModal")
  const form = document.getElementById("addWorkOrderForm")

  // Populate asset select
  const assetSelect = document.getElementById("assetSelect")
  assetSelect.innerHTML =
    '<option value="">Chọn tài sản</option>' +
    assets.map((a) => `<option value="${a.MaTaiSan}">${a.TenTaiSan}</option>`).join("")

  // Populate employee select
  const employeeSelect = document.getElementById("employeeSelect")
  employeeSelect.innerHTML =
    '<option value="">Chọn nhân viên</option>' +
    employees.map((e) => `<option value="${e.MaNhanVien}">${e.HoTen}</option>`).join("")

  modal.classList.remove("hidden")

  form.onsubmit = (e) => {
    e.preventDefault()
    const formData = new FormData(form)

    const newWorkOrder = {
      MaPhieu: "PCV" + Date.now(),
      MaTaiSan: formData.get("asset"),
      LoaiCongViec: formData.get("type"),
      MoTa: formData.get("description"),
      NgayBatDau: formData.get("startDate"),
      NgayKetThuc: formData.get("endDate"),
      MaNhanVienThucHien: formData.get("employee"),
      MucDoUuTien: formData.get("priority"),
      TrangThai: "Chờ xử lý",
      ChiPhi: Number.parseFloat(formData.get("cost")) || 0,
      GhiChu: formData.get("notes"),
    }

    addData("PhieuCongViec", newWorkOrder)
    modal.classList.add("hidden")
    form.reset()
    loadWorkOrders()
  }
}

function viewWorkOrder(id) {
  const workOrder = getData("PhieuCongViec").find((wo) => wo.MaPhieu === id)
  const asset = getData("TaiSan").find((a) => a.MaTaiSan === workOrder.MaTaiSan)
  const employee = getData("NhanVien").find((e) => e.MaNhanVien === workOrder.MaNhanVienThucHien)

  alert(
    `Chi tiết phiếu công việc:\n\nMã: ${workOrder.MaPhieu}\nTài sản: ${asset ? asset.TenTaiSan : "N/A"}\nLoại: ${workOrder.LoaiCongViec}\nMô tả: ${workOrder.MoTa}\nNhân viên: ${employee ? employee.HoTen : "N/A"}\nƯu tiên: ${workOrder.MucDoUuTien}\nTrạng thái: ${workOrder.TrangThai}\nChi phí: ${formatCurrency(workOrder.ChiPhi)}`,
  )
}

function editWorkOrder(id) {
  const workOrder = getData("PhieuCongViec").find((wo) => wo.MaPhieu === id)
  const newStatus = prompt("Nhập trạng thái mới (Chờ xử lý/Đang thực hiện/Hoàn thành/Đã hủy):", workOrder.TrangThai)

  if (newStatus && ["Chờ xử lý", "Đang thực hiện", "Hoàn thành", "Đã hủy"].includes(newStatus)) {
    workOrder.TrangThai = newStatus
    updateData("PhieuCongViec", id, workOrder)
    loadWorkOrders()
  }
}

function deleteWorkOrder(id) {
  if (confirm("Bạn có chắc chắn muốn xóa phiếu công việc này?")) {
    deleteData("PhieuCongViec", id)
    loadWorkOrders()
  }
}

function formatCurrency(amount) {
  return new Intl.NumberFormat("vi-VN", { style: "currency", currency: "VND" }).format(amount)
}

function closeModal(modalId) {
  document.getElementById(modalId).classList.add("hidden")
}
