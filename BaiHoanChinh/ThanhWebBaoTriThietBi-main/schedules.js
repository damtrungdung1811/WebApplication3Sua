// Schedules Page JavaScript
const currentUser = localStorage.getItem("currentUser")
let schedulesData = []
let assetsData = []
let employeesData = []

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
  existingData = existingData.map((item) => (item.MaLich === id ? newData : item))
  localStorage.setItem(key, JSON.stringify(existingData))
}

function deleteData(key, id) {
  let existingData = getData(key)
  existingData = existingData.filter((item) => item.MaLich !== id)
  localStorage.setItem(key, JSON.stringify(existingData))
}

function renderSchedules() {
  const tbody = document.getElementById("schedulesTableBody")
  const searchInput = document.getElementById("searchInput")
  const statusFilter = document.getElementById("statusFilter")

  if (!tbody || !searchInput || !statusFilter) return

  const searchTerm = searchInput.value.toLowerCase()
  const statusValue = statusFilter.value

  const filtered = schedulesData.filter((s) => {
    const asset = assetsData.find((a) => a.MaTaiSan === s.MaTaiSan)
    const matchesSearch =
      s.MaLich.toLowerCase().includes(searchTerm) || (asset && asset.TenTaiSan.toLowerCase().includes(searchTerm))
    const matchesStatus = !statusValue || s.TrangThai === statusValue
    return matchesSearch && matchesStatus
  })

  tbody.innerHTML = filtered
    .map((s) => {
      const asset = assetsData.find((a) => a.MaTaiSan === s.MaTaiSan)
      const employee = employeesData.find((e) => e.MaNhanVien === s.MaNhanVienPhuTrach)

      const statusColors = {
        "Đã lên lịch": "bg-blue-100 text-blue-800",
        "Đang thực hiện": "bg-yellow-100 text-yellow-800",
        "Hoàn thành": "bg-green-100 text-green-800",
        "Bỏ qua": "bg-red-100 text-red-800",
      }

      return `
                <tr class="hover:bg-gray-50">
                    <td class="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">${s.MaLich}</td>
                    <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">${asset ? asset.TenTaiSan : "N/A"}</td>
                    <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">${s.TanSuat}</td>
                    <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">${formatDate(s.NgayBaoTriTiepTheo)}</td>
                    <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">${employee ? employee.HoTen : "N/A"}</td>
                    <td class="px-6 py-4 whitespace-nowrap">
                        <span class="px-2 py-1 text-xs font-semibold rounded-full ${statusColors[s.TrangThai]}">${s.TrangThai}</span>
                    </td>
                    <td class="px-6 py-4 whitespace-nowrap text-sm font-medium">
                        <button onclick="viewSchedule('${s.MaLich}')" class="text-blue-600 hover:text-blue-900 mr-3">Xem</button>
                        <button onclick="editSchedule('${s.MaLich}')" class="text-indigo-600 hover:text-indigo-900 mr-3">Sửa</button>
                        <button onclick="deleteSchedule('${s.MaLich}')" class="text-red-600 hover:text-red-900">Xóa</button>
                    </td>
                </tr>
            `
    })
    .join("")

  if (filtered.length === 0) {
    tbody.innerHTML = '<tr><td colspan="7" class="px-6 py-4 text-center text-gray-500">Không có dữ liệu</td></tr>'
  }
}

document.addEventListener("DOMContentLoaded", () => {
  if (!checkAuth()) {
    window.location.href = "index.html"
    return
  }

  schedulesData = getData("LichBaoTri") || []
  assetsData = getData("TaiSan") || []
  employeesData = getData("NhanVien") || []

  const searchInput = document.getElementById("searchInput")
  const statusFilter = document.getElementById("statusFilter")

  searchInput.addEventListener("input", renderSchedules)
  statusFilter.addEventListener("change", renderSchedules)

  renderSchedules()

  document.getElementById("addScheduleBtn").addEventListener("click", showAddModal)
})

function showAddModal() {
  const modal = document.getElementById("addModal")
  const form = document.getElementById("addScheduleForm")

  const assetSelect = document.getElementById("assetSelect")
  assetSelect.innerHTML =
    '<option value="">Chọn tài sản</option>' +
    assetsData.map((a) => `<option value="${a.MaTaiSan}">${a.TenTaiSan}</option>`).join("")

  const employeeSelect = document.getElementById("employeeSelect")
  employeeSelect.innerHTML =
    '<option value="">Chọn nhân viên</option>' +
    employeesData.map((e) => `<option value="${e.MaNhanVien}">${e.HoTen}</option>`).join("")

  modal.classList.remove("hidden")

  form.onsubmit = (e) => {
    e.preventDefault()
    const formData = new FormData(form)

    const newSchedule = {
      MaLich: "LBT" + Date.now(),
      MaTaiSan: formData.get("asset"),
      TanSuat: formData.get("frequency"),
      NgayBaoTriTiepTheo: formData.get("nextDate"),
      MaNhanVienPhuTrach: formData.get("employee"),
      TrangThai: "Đã lên lịch",
      GhiChu: formData.get("notes"),
    }

    addData("LichBaoTri", newSchedule)
    schedulesData = getData("LichBaoTri")
    modal.classList.add("hidden")
    form.reset()
    renderSchedules()
  }
}

function viewSchedule(id) {
  const schedule = schedulesData.find((s) => s.MaLich === id)
  const asset = assetsData.find((a) => a.MaTaiSan === schedule.MaTaiSan)
  const employee = employeesData.find((e) => e.MaNhanVien === schedule.MaNhanVienPhuTrach)

  alert(
    `Chi tiết lịch bảo trì:\n\nMã: ${schedule.MaLich}\nTài sản: ${asset ? asset.TenTaiSan : "N/A"}\nTần suất: ${schedule.TanSuat}\nNgày tiếp theo: ${formatDate(schedule.NgayBaoTriTiepTheo)}\nNhân viên: ${employee ? employee.HoTen : "N/A"}\nTrạng thái: ${schedule.TrangThai}`,
  )
}

function editSchedule(id) {
  const schedule = schedulesData.find((s) => s.MaLich === id)
  const newStatus = prompt("Nhập trạng thái mới (Đã lên lịch/Đang thực hiện/Hoàn thành/Bỏ qua):", schedule.TrangThai)

  if (newStatus && ["Đã lên lịch", "Đang thực hiện", "Hoàn thành", "Bỏ qua"].includes(newStatus)) {
    schedule.TrangThai = newStatus
    updateData("LichBaoTri", id, schedule)
    schedulesData = getData("LichBaoTri")
    renderSchedules()
  }
}

function deleteSchedule(id) {
  if (confirm("Bạn có chắc chắn muốn xóa lịch bảo trì này?")) {
    deleteData("LichBaoTri", id)
    schedulesData = getData("LichBaoTri")
    renderSchedules()
  }
}

function formatDate(dateString) {
  return new Date(dateString).toLocaleDateString("vi-VN")
}

function closeModal(modalId) {
  document.getElementById(modalId).classList.add("hidden")
}
