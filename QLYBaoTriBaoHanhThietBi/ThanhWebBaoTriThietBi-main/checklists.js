// Checklists Page JavaScript
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
  existingData = existingData.map((item) => (item.MaChecklist === id ? newData : item))
  localStorage.setItem(key, JSON.stringify(existingData))
}

function deleteData(key, id) {
  let existingData = getData(key)
  existingData = existingData.filter((item) => item.MaChecklist !== id)
  localStorage.setItem(key, JSON.stringify(existingData))
}

document.addEventListener("DOMContentLoaded", () => {
  if (!checkAuth()) {
    window.location.href = "index.html"
    return
  }

  loadChecklists()
  setupEventListeners()
})

function loadChecklists() {
  const checklists = getData("Checklist") || []
  const items = getData("ChecklistItem") || []

  const grid = document.getElementById("checklistsGrid")

  grid.innerHTML = checklists
    .map((checklist) => {
      const checklistItems = items.filter((i) => i.MaChecklist === checklist.MaChecklist)

      return `
            <div class="bg-white rounded-lg shadow-md p-6">
                <h3 class="text-xl font-bold text-gray-800 mb-2">${checklist.TenChecklist}</h3>
                <p class="text-sm text-gray-600 mb-4">${checklist.MoTa || "Không có mô tả"}</p>
                <div class="mb-4">
                    <span class="text-sm text-gray-500">${checklistItems.length} mục kiểm tra</span>
                </div>
                <div class="flex gap-2">
                    <button onclick="viewChecklist('${checklist.MaChecklist}')" class="flex-1 bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600">Xem</button>
                    <button onclick="editChecklist('${checklist.MaChecklist}')" class="flex-1 bg-indigo-500 text-white px-4 py-2 rounded hover:bg-indigo-600">Sửa</button>
                    <button onclick="deleteChecklist('${checklist.MaChecklist}')" class="flex-1 bg-red-500 text-white px-4 py-2 rounded hover:bg-red-600">Xóa</button>
                </div>
            </div>
        `
    })
    .join("")

  if (checklists.length === 0) {
    grid.innerHTML = '<div class="col-span-full text-center text-gray-500 py-8">Không có dữ liệu</div>'
  }
}

function setupEventListeners() {
  document.getElementById("addChecklistBtn").addEventListener("click", showAddModal)
}

function showAddModal() {
  const modal = document.getElementById("addModal")
  const form = document.getElementById("addChecklistForm")

  modal.classList.remove("hidden")

  form.onsubmit = (e) => {
    e.preventDefault()
    const formData = new FormData(form)

    const newChecklist = {
      MaChecklist: "CL" + Date.now(),
      TenChecklist: formData.get("name"),
      MoTa: formData.get("description"),
    }

    addData("Checklist", newChecklist)
    modal.classList.add("hidden")
    form.reset()
    loadChecklists()
  }
}

function viewChecklist(id) {
  const checklist = getData("Checklist").find((c) => c.MaChecklist === id)
  const items = getData("ChecklistItem").filter((i) => i.MaChecklist === id)

  const itemsList = items.map((i) => `- ${i.NoiDung}`).join("\n")

  alert(
    `Chi tiết checklist:\n\nMã: ${checklist.MaChecklist}\nTên: ${checklist.TenChecklist}\nMô tả: ${checklist.MoTa || "Không có"}\n\nCác mục:\n${itemsList || "Chưa có mục nào"}`,
  )
}

function editChecklist(id) {
  const checklist = getData("Checklist").find((c) => c.MaChecklist === id)
  const newDescription = prompt("Nhập mô tả mới:", checklist.MoTa || "")

  if (newDescription !== null) {
    checklist.MoTa = newDescription
    updateData("Checklist", id, checklist)
    loadChecklists()
  }
}

function deleteChecklist(id) {
  if (confirm("Bạn có chắc chắn muốn xóa checklist này?")) {
    deleteData("Checklist", id)
    loadChecklists()
  }
}

function closeModal(modalId) {
  document.getElementById(modalId).classList.add("hidden")
}

function logout() {
  localStorage.removeItem("currentUser")
  window.location.href = "index.html"
}
