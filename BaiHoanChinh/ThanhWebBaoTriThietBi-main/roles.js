// Roles Page JavaScript
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
  existingData = existingData.map((item) => (item.MaVaiTro === id ? newData : item))
  localStorage.setItem(key, JSON.stringify(existingData))
}

function deleteData(key, id) {
  let existingData = getData(key)
  existingData = existingData.filter((item) => item.MaVaiTro !== id)
  localStorage.setItem(key, JSON.stringify(existingData))
}

document.addEventListener("DOMContentLoaded", () => {
  if (!checkAuth()) {
    window.location.href = "index.html"
    return
  }

  loadRoles()
  setupEventListeners()
})

function loadRoles() {
  const roles = getData("VaiTro") || []
  const users = getData("NguoiDung") || []

  const grid = document.getElementById("rolesGrid")

  grid.innerHTML = roles
    .map((role) => {
      const userCount = users.filter((u) => u.MaVaiTro === role.MaVaiTro).length

      return `
            <div class="bg-white rounded-lg shadow-md p-6">
                <div class="flex justify-between items-start mb-4">
                    <div>
                        <h3 class="text-xl font-bold text-gray-800">${role.TenVaiTro}</h3>
                        <p class="text-sm text-gray-600 mt-1">${role.MoTa || "Không có mô tả"}</p>
                    </div>
                    <span class="bg-blue-100 text-blue-800 text-xs font-semibold px-2.5 py-0.5 rounded">${userCount} người dùng</span>
                </div>
                <div class="flex gap-2 mt-4">
                    <button onclick="viewRole('${role.MaVaiTro}')" class="flex-1 bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600">Xem</button>
                    <button onclick="editRole('${role.MaVaiTro}')" class="flex-1 bg-indigo-500 text-white px-4 py-2 rounded hover:bg-indigo-600">Sửa</button>
                    <button onclick="deleteRole('${role.MaVaiTro}')" class="flex-1 bg-red-500 text-white px-4 py-2 rounded hover:bg-red-600">Xóa</button>
                </div>
            </div>
        `
    })
    .join("")

  if (roles.length === 0) {
    grid.innerHTML = '<div class="col-span-full text-center text-gray-500 py-8">Không có dữ liệu</div>'
  }
}

function setupEventListeners() {
  document.getElementById("addRoleBtn").addEventListener("click", showAddModal)
}

function showAddModal() {
  const modal = document.getElementById("addModal")
  const form = document.getElementById("addRoleForm")

  modal.classList.remove("hidden")

  form.onsubmit = (e) => {
    e.preventDefault()
    const formData = new FormData(form)

    const newRole = {
      MaVaiTro: "VT" + Date.now(),
      TenVaiTro: formData.get("name"),
      MoTa: formData.get("description"),
    }

    addData("VaiTro", newRole)
    modal.classList.add("hidden")
    form.reset()
    loadRoles()
  }
}

function viewRole(id) {
  const role = getData("VaiTro").find((r) => r.MaVaiTro === id)
  const users = getData("NguoiDung").filter((u) => u.MaVaiTro === id)

  alert(
    `Chi tiết vai trò:\n\nMã: ${role.MaVaiTro}\nTên: ${role.TenVaiTro}\nMô tả: ${role.MoTa || "Không có"}\nSố người dùng: ${users.length}`,
  )
}

function editRole(id) {
  const role = getData("VaiTro").find((r) => r.MaVaiTro === id)
  const newDescription = prompt("Nhập mô tả mới:", role.MoTa || "")

  if (newDescription !== null) {
    role.MoTa = newDescription
    updateData("VaiTro", id, role)
    loadRoles()
  }
}

function deleteRole(id) {
  if (confirm("Bạn có chắc chắn muốn xóa vai trò này?")) {
    deleteData("VaiTro", id)
    loadRoles()
  }
}

function closeModal(modalId) {
  document.getElementById(modalId).classList.add("hidden")
}

function logout() {
  localStorage.removeItem("currentUser")
  window.location.href = "index.html"
}
