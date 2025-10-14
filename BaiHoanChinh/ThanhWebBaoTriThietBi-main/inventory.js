// Inventory Page JavaScript
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
  existingData = existingData.map((item) => (item.MaLinhKien === id ? newData : item))
  localStorage.setItem(key, JSON.stringify(existingData))
}

function deleteData(key, id) {
  let existingData = getData(key)
  existingData = existingData.filter((item) => item.MaLinhKien !== id)
  localStorage.setItem(key, JSON.stringify(existingData))
}

document.addEventListener("DOMContentLoaded", () => {
  if (!checkAuth()) {
    window.location.href = "index.html"
    return
  }

  loadInventory()
  setupEventListeners()
})

function loadInventory() {
  const inventory = getData("LinhKien") || []

  const tbody = document.getElementById("inventoryTableBody")
  const searchInput = document.getElementById("searchInput")
  const statusFilter = document.getElementById("statusFilter")

  function renderInventory() {
    const searchTerm = searchInput.value.toLowerCase()
    const statusValue = statusFilter.value

    const filtered = inventory.filter((item) => {
      const matchesSearch =
        item.TenLinhKien.toLowerCase().includes(searchTerm) || item.MaLinhKien.toLowerCase().includes(searchTerm)

      let itemStatus = "Còn hàng"
      if (item.SoLuongTonKho === 0) itemStatus = "Hết hàng"
      else if (item.SoLuongTonKho <= item.SoLuongToiThieu) itemStatus = "Sắp hết"

      const matchesStatus = !statusValue || itemStatus === statusValue
      return matchesSearch && matchesStatus
    })

    tbody.innerHTML = filtered
      .map((item) => {
        let status = "Còn hàng"
        let statusColor = "bg-green-100 text-green-800"

        if (item.SoLuongTonKho === 0) {
          status = "Hết hàng"
          statusColor = "bg-red-100 text-red-800"
        } else if (item.SoLuongTonKho <= item.SoLuongToiThieu) {
          status = "Sắp hết"
          statusColor = "bg-yellow-100 text-yellow-800"
        }

        return `
                <tr class="hover:bg-gray-50">
                    <td class="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">${item.MaLinhKien}</td>
                    <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">${item.TenLinhKien}</td>
                    <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">${item.LoaiLinhKien}</td>
                    <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">${item.SoLuongTonKho}</td>
                    <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">${item.DonVi}</td>
                    <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">${formatCurrency(item.DonGia)}</td>
                    <td class="px-6 py-4 whitespace-nowrap">
                        <span class="px-2 py-1 text-xs font-semibold rounded-full ${statusColor}">${status}</span>
                    </td>
                    <td class="px-6 py-4 whitespace-nowrap text-sm font-medium">
                        <button onclick="viewInventory('${item.MaLinhKien}')" class="text-blue-600 hover:text-blue-900 mr-3">Xem</button>
                        <button onclick="editInventory('${item.MaLinhKien}')" class="text-indigo-600 hover:text-indigo-900 mr-3">Sửa</button>
                        <button onclick="deleteInventory('${item.MaLinhKien}')" class="text-red-600 hover:text-red-900">Xóa</button>
                    </td>
                </tr>
            `
      })
      .join("")

    if (filtered.length === 0) {
      tbody.innerHTML = '<tr><td colspan="8" class="px-6 py-4 text-center text-gray-500">Không có dữ liệu</td></tr>'
    }
  }

  searchInput.addEventListener("input", renderInventory)
  statusFilter.addEventListener("change", renderInventory)

  renderInventory()
}

function setupEventListeners() {
  document.getElementById("addInventoryBtn").addEventListener("click", showAddModal)
}

function showAddModal() {
  const modal = document.getElementById("addModal")
  const form = document.getElementById("addInventoryForm")

  modal.classList.remove("hidden")

  form.onsubmit = (e) => {
    e.preventDefault()
    const formData = new FormData(form)

    const newItem = {
      MaLinhKien: "LK" + Date.now(),
      TenLinhKien: formData.get("name"),
      LoaiLinhKien: formData.get("type"),
      SoLuongTonKho: Number.parseInt(formData.get("quantity")),
      DonVi: formData.get("unit"),
      DonGia: Number.parseFloat(formData.get("price")),
      SoLuongToiThieu: 10,
    }

    addData("LinhKien", newItem)
    modal.classList.add("hidden")
    form.reset()
    loadInventory()
  }
}

function viewInventory(id) {
  const item = getData("LinhKien").find((i) => i.MaLinhKien === id)

  let status = "Còn hàng"
  if (item.SoLuongTonKho === 0) status = "Hết hàng"
  else if (item.SoLuongTonKho <= item.SoLuongToiThieu) status = "Sắp hết"

  alert(
    `Chi tiết linh kiện:\n\nMã: ${item.MaLinhKien}\nTên: ${item.TenLinhKien}\nLoại: ${item.LoaiLinhKien}\nSố lượng: ${item.SoLuongTonKho} ${item.DonVi}\nĐơn giá: ${formatCurrency(item.DonGia)}\nTrạng thái: ${status}`,
  )
}

function editInventory(id) {
  const item = getData("LinhKien").find((i) => i.MaLinhKien === id)
  const newQuantity = prompt("Nhập số lượng mới:", item.SoLuongTonKho)

  if (newQuantity !== null && !isNaN(newQuantity)) {
    item.SoLuongTonKho = Number.parseInt(newQuantity)
    updateData("LinhKien", id, item)
    loadInventory()
  }
}

function deleteInventory(id) {
  if (confirm("Bạn có chắc chắn muốn xóa linh kiện này?")) {
    deleteData("LinhKien", id)
    loadInventory()
  }
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
