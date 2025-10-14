// Incidents Page JavaScript
const currentUser = localStorage.getItem("currentUser")

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
  const checkAuth = () => {
    return currentUser !== null
  }

  if (!checkAuth()) {
    window.location.href = "index.html"
    return
  }

  loadIncidents()
  setupEventListeners()
})

function loadIncidents() {
  const incidents = getData("PhieuSuCo") || []
  const assets = getData("TaiSan") || []
  const employees = getData("NhanVien") || []

  const tbody = document.getElementById("incidentsTableBody")
  const searchInput = document.getElementById("searchInput")
  const statusFilter = document.getElementById("statusFilter")
  const severityFilter = document.getElementById("severityFilter")

  function renderIncidents() {
    const searchTerm = searchInput.value.toLowerCase()
    const statusValue = statusFilter.value
    const severityValue = severityFilter.value

    const filtered = incidents.filter((i) => {
      const matchesSearch = i.MoTa.toLowerCase().includes(searchTerm) || i.MaPhieu.toLowerCase().includes(searchTerm)
      const matchesStatus = !statusValue || i.TrangThai === statusValue
      const matchesSeverity = !severityValue || i.MucDoNghiemTrong === severityValue
      return matchesSearch && matchesStatus && matchesSeverity
    })

    tbody.innerHTML = filtered
      .map((i) => {
        const asset = assets.find((a) => a.MaTaiSan === i.MaTaiSan)
        const reporter = employees.find((e) => e.MaNhanVien === i.NguoiBaoCao)
        const handler = employees.find((e) => e.MaNhanVien === i.NguoiXuLy)

        const statusColors = {
          Mới: "bg-yellow-100 text-yellow-800",
          "Đang xử lý": "bg-blue-100 text-blue-800",
          "Đã xử lý": "bg-green-100 text-green-800",
          "Đã đóng": "bg-gray-100 text-gray-800",
        }

        const severityColors = {
          Thấp: "bg-green-100 text-green-800",
          "Trung bình": "bg-yellow-100 text-yellow-800",
          Cao: "bg-orange-100 text-orange-800",
          "Nghiêm trọng": "bg-red-100 text-red-800",
        }

        return `
                <tr class="hover:bg-gray-50">
                    <td class="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">${i.MaPhieu}</td>
                    <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">${asset ? asset.TenTaiSan : "N/A"}</td>
                    <td class="px-6 py-4 text-sm text-gray-900">${i.MoTa}</td>
                    <td class="px-6 py-4 whitespace-nowrap">
                        <span class="px-2 py-1 text-xs font-semibold rounded-full ${severityColors[i.MucDoNghiemTrong]}">${i.MucDoNghiemTrong}</span>
                    </td>
                    <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">${reporter ? reporter.HoTen : "N/A"}</td>
                    <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">${handler ? handler.HoTen : "Chưa phân công"}</td>
                    <td class="px-6 py-4 whitespace-nowrap">
                        <span class="px-2 py-1 text-xs font-semibold rounded-full ${statusColors[i.TrangThai]}">${i.TrangThai}</span>
                    </td>
                    <td class="px-6 py-4 whitespace-nowrap text-sm font-medium">
                        <button onclick="viewIncident('${i.MaPhieu}')" class="text-blue-600 hover:text-blue-900 mr-3">Xem</button>
                        <button onclick="editIncident('${i.MaPhieu}')" class="text-indigo-600 hover:text-indigo-900 mr-3">Sửa</button>
                        <button onclick="deleteIncident('${i.MaPhieu}')" class="text-red-600 hover:text-red-900">Xóa</button>
                    </td>
                </tr>
            `
      })
      .join("")

    if (filtered.length === 0) {
      tbody.innerHTML = '<tr><td colspan="8" class="px-6 py-4 text-center text-gray-500">Không có dữ liệu</td></tr>'
    }
  }

  searchInput.addEventListener("input", renderIncidents)
  statusFilter.addEventListener("change", renderIncidents)
  severityFilter.addEventListener("change", renderIncidents)

  renderIncidents()
}

function setupEventListeners() {
  document.getElementById("addIncidentBtn").addEventListener("click", showAddModal)
}

function showAddModal() {
  const assets = getData("TaiSan") || []
  const employees = getData("NhanVien") || []

  const modal = document.getElementById("addModal")
  const form = document.getElementById("addIncidentForm")

  const assetSelect = document.getElementById("assetSelect")
  assetSelect.innerHTML =
    '<option value="">Chọn tài sản</option>' +
    assets.map((a) => `<option value="${a.MaTaiSan}">${a.TenTaiSan}</option>`).join("")

  const reporterSelect = document.getElementById("reporterSelect")
  reporterSelect.innerHTML =
    '<option value="">Chọn người báo cáo</option>' +
    employees.map((e) => `<option value="${e.MaNhanVien}">${e.HoTen}</option>`).join("")

  modal.classList.remove("hidden")

  form.onsubmit = (e) => {
    e.preventDefault()
    const formData = new FormData(form)

    const newIncident = {
      MaPhieu: "PSC" + Date.now(),
      MaTaiSan: formData.get("asset"),
      MoTa: formData.get("description"),
      MucDoNghiemTrong: formData.get("severity"),
      NgayPhatHien: formData.get("date"),
      NguoiBaoCao: formData.get("reporter"),
      TrangThai: "Mới",
      GhiChu: formData.get("notes"),
    }

    addData("PhieuSuCo", newIncident)
    modal.classList.add("hidden")
    form.reset()
    loadIncidents()
  }
}

function viewIncident(id) {
  const incident = getData("PhieuSuCo").find((i) => i.MaPhieu === id)
  const asset = getData("TaiSan").find((a) => a.MaTaiSan === incident.MaTaiSan)
  const reporter = getData("NhanVien").find((e) => e.MaNhanVien === incident.NguoiBaoCao)

  alert(
    `Chi tiết sự cố:\n\nMã: ${incident.MaPhieu}\nTài sản: ${asset ? asset.TenTaiSan : "N/A"}\nMô tả: ${incident.MoTa}\nMức độ: ${incident.MucDoNghiemTrong}\nNgười báo cáo: ${reporter ? reporter.HoTen : "N/A"}\nTrạng thái: ${incident.TrangThai}`,
  )
}

function editIncident(id) {
  const incident = getData("PhieuSuCo").find((i) => i.MaPhieu === id)
  const newStatus = prompt("Nhập trạng thái mới (Mới/Đang xử lý/Đã xử lý/Đã đóng):", incident.TrangThai)

  if (newStatus && ["Mới", "Đang xử lý", "Đã xử lý", "Đã đóng"].includes(newStatus)) {
    incident.TrangThai = newStatus
    updateData("PhieuSuCo", id, incident)
    loadIncidents()
  }
}

function deleteIncident(id) {
  if (confirm("Bạn có chắc chắn muốn xóa sự cố này?")) {
    deleteData("PhieuSuCo", id)
    loadIncidents()
  }
}

function closeModal(modalId) {
  document.getElementById(modalId).classList.add("hidden")
}
