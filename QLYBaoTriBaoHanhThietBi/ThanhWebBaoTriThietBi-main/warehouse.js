// Warehouse Page JavaScript
const currentUser = localStorage.getItem("currentUser")

function checkAuth() {
  return currentUser !== null
}

function getData(key) {
  return JSON.parse(localStorage.getItem(key)) || []
}

function addData(key, data) {
  const existingData = getData(key)
  existingData.push(data)
  localStorage.setItem(key, JSON.stringify(existingData))
}

function updateData(key, id, newData) {
  let existingData = getData(key)
  existingData = existingData.map((item) => (item.MaPhieu === id ? newData : item))
  localStorage.setItem(key, JSON.stringify(existingData))
}

function deleteData(key, id) {
  let existingData = getData(key)
  existingData = existingData.filter((item) => item.MaPhieu !== id)
  localStorage.setItem(key, JSON.stringify(existingData))
}

document.addEventListener("DOMContentLoaded", () => {
  if (!checkAuth()) {
    window.location.href = "index.html"
    return
  }

  loadWarehouse()
  setupEventListeners()
})

function loadWarehouse() {
  const warehouse = getData("PhieuKho") || []
  const employees = getData("NhanVien") || []

  const tbody = document.getElementById("warehouseTableBody")
  const searchInput = document.getElementById("searchInput")
  const typeFilter = document.getElementById("typeFilter")

  function renderWarehouse() {
    const searchTerm = searchInput.value.toLowerCase()
    const typeValue = typeFilter.value

    const filtered = warehouse.filter((w) => {
      const matchesSearch = w.MaPhieu.toLowerCase().includes(searchTerm)
      const matchesType = !typeValue || w.LoaiPhieu === typeValue
      return matchesSearch && matchesType
    })

    tbody.innerHTML = filtered
      .map((w) => {
        const employee = employees.find((e) => e.MaNhanVien === w.NguoiLap)

        const typeColors = {
          Nhập: "bg-green-100 text-green-800",
          Xuất: "bg-blue-100 text-blue-800",
        }

        return `
                <tr class="hover:bg-gray-50">
                    <td class="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">${w.MaPhieu}</td>
                    <td class="px-6 py-4 whitespace-nowrap">
                        <span class="px-2 py-1 text-xs font-semibold rounded-full ${typeColors[w.LoaiPhieu]}">${w.LoaiPhieu}</span>
                    </td>
                    <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">${formatDate(w.NgayLap)}</td>
                    <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">${employee ? employee.HoTen : "N/A"}</td>
                    <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">${formatCurrency(w.TongGiaTri || 0)}</td>
                    <td class="px-6 py-4 whitespace-nowrap text-sm font-medium">
                        <button onclick="viewWarehouse('${w.MaPhieu}')" class="text-blue-600 hover:text-blue-900 mr-3">Xem</button>
                        <button onclick="editWarehouse('${w.MaPhieu}')" class="text-indigo-600 hover:text-indigo-900 mr-3">Sửa</button>
                        <button onclick="deleteWarehouse('${w.MaPhieu}')" class="text-red-600 hover:text-red-900">Xóa</button>
                    </td>
                </tr>
            `
      })
      .join("")

    if (filtered.length === 0) {
      tbody.innerHTML = '<tr><td colspan="6" class="px-6 py-4 text-center text-gray-500">Không có dữ liệu</td></tr>'
    }
  }

  searchInput.addEventListener("input", renderWarehouse)
  typeFilter.addEventListener("change", renderWarehouse)

  renderWarehouse()
}

function setupEventListeners() {
  document.getElementById("addWarehouseBtn").addEventListener("click", showAddModal)
}

function showAddModal() {
  const modal = document.getElementById("addModal")
  const form = document.getElementById("addWarehouseForm")

  modal.classList.remove("hidden")

  form.onsubmit = (e) => {
    e.preventDefault()
    const formData = new FormData(form)

    const newWarehouse = {
      MaPhieu: "PK" + Date.now(),
      LoaiPhieu: formData.get("type"),
      NgayLap: formData.get("date"),
      NguoiLap: currentUser,
      TongGiaTri: 0,
      GhiChu: formData.get("notes"),
    }

    addData("PhieuKho", newWarehouse)
    modal.classList.add("hidden")
    form.reset()
    loadWarehouse()
  }
}

function viewWarehouse(id) {
  const warehouse = getData("PhieuKho").find((w) => w.MaPhieu === id)
  const employee = getData("NhanVien").find((e) => e.MaNhanVien === warehouse.NguoiLap)

  alert(
    `Chi tiết phiếu kho:\n\nMã: ${warehouse.MaPhieu}\nLoại: ${warehouse.LoaiPhieu}\nNgày lập: ${formatDate(warehouse.NgayLap)}\nNgười lập: ${employee ? employee.HoTen : "N/A"}\nTổng giá trị: ${formatCurrency(warehouse.TongGiaTri)}\nGhi chú: ${warehouse.GhiChu || "Không có"}`,
  )
}

function editWarehouse(id) {
  const warehouse = getData("PhieuKho").find((w) => w.MaPhieu === id)
  const newNotes = prompt("Nhập ghi chú mới:", warehouse.GhiChu || "")

  if (newNotes !== null) {
    warehouse.GhiChu = newNotes
    updateData("PhieuKho", id, warehouse)
    loadWarehouse()
  }
}

function deleteWarehouse(id) {
  if (confirm("Bạn có chắc chắn muốn xóa phiếu kho này?")) {
    deleteData("PhieuKho", id)
    loadWarehouse()
  }
}

function formatDate(dateString) {
  return new Date(dateString).toLocaleDateString("vi-VN")
}

function formatCurrency(amount) {
  return new Intl.NumberFormat("vi-VN", { style: "currency", currency: "VND" }).format(amount)
}

function closeModal(modalId) {
  document.getElementById(modalId).classList.add("hidden")
}

function logout() {
  localStorage.removeItem("currentUser")
  window.location.href = "index.html"
}
