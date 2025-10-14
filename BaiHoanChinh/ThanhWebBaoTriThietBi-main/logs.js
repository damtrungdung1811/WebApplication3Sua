// Logs Page JavaScript
const currentUser = localStorage.getItem("currentUser")

function checkAuth() {
  return currentUser !== null
}

function getData(key) {
  return JSON.parse(localStorage.getItem(key)) || []
}

document.addEventListener("DOMContentLoaded", () => {
  if (!checkAuth()) {
    window.location.href = "index.html"
    return
  }

  loadLogs()
  setupEventListeners()
})

function loadLogs() {
  const logs = getData("NhatKyHeThong") || []
  const users = getData("NguoiDung") || []

  const tbody = document.getElementById("logsTableBody")
  const searchInput = document.getElementById("searchInput")
  const actionFilter = document.getElementById("actionFilter")
  const dateFilter = document.getElementById("dateFilter")

  function renderLogs() {
    const searchTerm = searchInput.value.toLowerCase()
    const actionValue = actionFilter.value
    const dateValue = dateFilter.value

    const filtered = logs.filter((log) => {
      const matchesSearch =
        log.MoTa.toLowerCase().includes(searchTerm) || log.HanhDong.toLowerCase().includes(searchTerm)
      const matchesAction = !actionValue || log.HanhDong === actionValue
      const matchesDate = !dateValue || log.ThoiGian.startsWith(dateValue)
      return matchesSearch && matchesAction && matchesDate
    })

    tbody.innerHTML = filtered
      .map((log) => {
        const user = users.find((u) => u.MaNguoiDung === log.MaNguoiDung)

        const actionColors = {
          Thêm: "bg-green-100 text-green-800",
          Sửa: "bg-blue-100 text-blue-800",
          Xóa: "bg-red-100 text-red-800",
          "Đăng nhập": "bg-purple-100 text-purple-800",
          "Đăng xuất": "bg-gray-100 text-gray-800",
        }

        return `
                <tr class="hover:bg-gray-50">
                    <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">${formatDateTime(log.ThoiGian)}</td>
                    <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">${user ? user.TenDangNhap : "N/A"}</td>
                    <td class="px-6 py-4 whitespace-nowrap">
                        <span class="px-2 py-1 text-xs font-semibold rounded-full ${actionColors[log.HanhDong] || "bg-gray-100 text-gray-800"}">${log.HanhDong}</span>
                    </td>
                    <td class="px-6 py-4 text-sm text-gray-900">${log.MoTa}</td>
                </tr>
            `
      })
      .join("")

    if (filtered.length === 0) {
      tbody.innerHTML = '<tr><td colspan="4" class="px-6 py-4 text-center text-gray-500">Không có dữ liệu</td></tr>'
    }
  }

  searchInput.addEventListener("input", renderLogs)
  actionFilter.addEventListener("change", renderLogs)
  dateFilter.addEventListener("change", renderLogs)

  renderLogs()
}

function setupEventListeners() {
  // No additional event listeners needed for logs page (read-only)
}

function formatDateTime(dateTimeString) {
  const date = new Date(dateTimeString)
  return date.toLocaleString("vi-VN")
}

function logout() {
  localStorage.removeItem("currentUser")
  window.location.href = "index.html"
}
