// Employees Page JavaScript
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
  existingData = existingData.map((item) => (item.MaNhanVien === id ? newData : item))
  localStorage.setItem(key, JSON.stringify(existingData))
}

function deleteData(key, id) {
  let existingData = getData(key)
  existingData = existingData.filter((item) => item.MaNhanVien !== id)
  localStorage.setItem(key, JSON.stringify(existingData))
}

document.addEventListener("DOMContentLoaded", () => {
  if (!checkAuth()) {
    window.location.href = "index.html"
    return
  }

  loadEmployees()
  setupEventListeners()
})

function loadEmployees() {
  const employees = getData("NhanVien") || []

  const tbody = document.getElementById("employeesTableBody")
  const searchInput = document.getElementById("searchInput")
  const statusFilter = document.getElementById("statusFilter")

  function renderEmployees() {
    const searchTerm = searchInput.value.toLowerCase()
    const statusValue = statusFilter.value

    const filtered = employees.filter((e) => {
      const matchesSearch =
        e.HoTen.toLowerCase().includes(searchTerm) ||
        e.MaNhanVien.toLowerCase().includes(searchTerm) ||
        e.Email.toLowerCase().includes(searchTerm)
      const matchesStatus = !statusValue || e.TrangThai === statusValue
      return matchesSearch && matchesStatus
    })

    tbody.innerHTML = filtered
      .map((e) => {
        const statusColors = {
          "Đang làm việc": "bg-green-100 text-green-800",
          "Nghỉ việc": "bg-red-100 text-red-800",
        }

        return `
                <tr class="hover:bg-gray-50">
                    <td class="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">${e.MaNhanVien}</td>
                    <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">${e.HoTen}</td>
                    <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">${e.ChucVu}</td>
                    <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">${e.Email}</td>
                    <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">${e.SoDienThoai}</td>
                    <td class="px-6 py-4 whitespace-nowrap">
                        <span class="px-2 py-1 text-xs font-semibold rounded-full ${statusColors[e.TrangThai]}">${e.TrangThai}</span>
                    </td>
                    <td class="px-6 py-4 whitespace-nowrap text-sm font-medium">
                        <button onclick="viewEmployee('${e.MaNhanVien}')" class="text-blue-600 hover:text-blue-900 mr-3">Xem</button>
                        <button onclick="editEmployee('${e.MaNhanVien}')" class="text-indigo-600 hover:text-indigo-900 mr-3">Sửa</button>
                        <button onclick="deleteEmployee('${e.MaNhanVien}')" class="text-red-600 hover:text-red-900">Xóa</button>
                    </td>
                </tr>
            `
      })
      .join("")

    if (filtered.length === 0) {
      tbody.innerHTML = '<tr><td colspan="7" class="px-6 py-4 text-center text-gray-500">Không có dữ liệu</td></tr>'
    }
  }

  searchInput.addEventListener("input", renderEmployees)
  statusFilter.addEventListener("change", renderEmployees)

  renderEmployees()
}

function setupEventListeners() {
  document.getElementById("addEmployeeBtn").addEventListener("click", showAddModal)
}

function showAddModal() {
  const modal = document.getElementById("addModal")
  const form = document.getElementById("addEmployeeForm")

  modal.classList.remove("hidden")

  form.onsubmit = (e) => {
    e.preventDefault()
    const formData = new FormData(form)

    const newEmployee = {
      MaNhanVien: "NV" + Date.now(),
      HoTen: formData.get("name"),
      ChucVu: formData.get("position"),
      Email: formData.get("email"),
      SoDienThoai: formData.get("phone"),
      TrangThai: "Đang làm việc",
    }

    addData("NhanVien", newEmployee)
    modal.classList.add("hidden")
    form.reset()
    loadEmployees()
  }
}

function viewEmployee(id) {
  const employee = getData("NhanVien").find((e) => e.MaNhanVien === id)

  alert(
    `Chi tiết nhân viên:\n\nMã: ${employee.MaNhanVien}\nHọ tên: ${employee.HoTen}\nChức vụ: ${employee.ChucVu}\nEmail: ${employee.Email}\nSố điện thoại: ${employee.SoDienThoai}\nTrạng thái: ${employee.TrangThai}`,
  )
}

function editEmployee(id) {
  const employee = getData("NhanVien").find((e) => e.MaNhanVien === id)
  const newStatus = prompt("Nhập trạng thái mới (Đang làm việc/Nghỉ việc):", employee.TrangThai)

  if (newStatus && ["Đang làm việc", "Nghỉ việc"].includes(newStatus)) {
    employee.TrangThai = newStatus
    updateData("NhanVien", id, employee)
    loadEmployees()
  }
}

function deleteEmployee(id) {
  if (confirm("Bạn có chắc chắn muốn xóa nhân viên này?")) {
    deleteData("NhanVien", id)
    loadEmployees()
  }
}

function closeModal(modalId) {
  document.getElementById(modalId).classList.add("hidden")
}

function logout() {
  localStorage.removeItem("currentUser")
  window.location.href = "index.html"
}
