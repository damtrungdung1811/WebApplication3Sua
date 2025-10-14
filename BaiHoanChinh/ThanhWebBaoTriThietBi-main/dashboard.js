// Kiểm tra đăng nhập
if (!localStorage.getItem("currentUser")) {
  window.location.href = "index.html"
}

// Hiển thị thông tin người dùng
const currentUser = JSON.parse(localStorage.getItem("currentUser"))
document.getElementById("userName").textContent = currentUser.HoTen
document.getElementById("userRole").textContent = currentUser.VaiTro

// Xử lý đăng xuất
document.getElementById("logoutBtn").addEventListener("click", () => {
  localStorage.removeItem("currentUser")
  window.location.href = "index.html"
})

// Tải dữ liệu thống kê
function loadDashboardStats() {
  const assets = JSON.parse(localStorage.getItem("assets") || "[]")
  const workOrders = JSON.parse(localStorage.getItem("workOrders") || "[]")
  const schedules = JSON.parse(localStorage.getItem("schedules") || "[]")
  const incidents = JSON.parse(localStorage.getItem("incidents") || "[]")

  // Cập nhật số liệu
  document.getElementById("totalAssets").textContent = assets.length
  document.getElementById("totalWorkOrders").textContent = workOrders.length
  document.getElementById("totalSchedules").textContent = schedules.length
  document.getElementById("totalIncidents").textContent = incidents.filter((i) => i.TrangThai === "Đang xử lý").length

  // Tải danh sách hoạt động gần đây
  loadRecentActivities()

  // Tải cảnh báo
  loadAlerts()
}

function loadRecentActivities() {
  const workOrders = JSON.parse(localStorage.getItem("workOrders") || "[]")
  const incidents = JSON.parse(localStorage.getItem("incidents") || "[]")

  const activities = [
    ...workOrders.slice(0, 3).map((wo) => ({
      type: "work-order",
      title: wo.TieuDe,
      time: wo.NgayTao,
      status: wo.TrangThai,
    })),
    ...incidents.slice(0, 2).map((inc) => ({
      type: "incident",
      title: inc.TieuDe,
      time: inc.NgayBaoCao,
      status: inc.TrangThai,
    })),
  ]

  const activityList = document.getElementById("activityList")
  activityList.innerHTML = activities
    .map(
      (act) => `
        <div class="activity-item">
            <div class="activity-icon ${act.type}"></div>
            <div class="activity-content">
                <h4>${act.title}</h4>
                <p>${new Date(act.time).toLocaleString("vi-VN")}</p>
            </div>
            <span class="badge ${act.status.toLowerCase().replace(" ", "-")}">${act.status}</span>
        </div>
    `,
    )
    .join("")
}

function loadAlerts() {
  const schedules = JSON.parse(localStorage.getItem("schedules") || "[]")
  const warranties = JSON.parse(localStorage.getItem("warranties") || "[]")
  const inventory = JSON.parse(localStorage.getItem("inventory") || "[]")

  const alerts = []

  // Cảnh báo lịch bảo trì sắp tới
  const upcomingSchedules = schedules.filter((s) => {
    const nextDate = new Date(s.NgayBaoTriTiepTheo)
    const today = new Date()
    const diff = (nextDate - today) / (1000 * 60 * 60 * 24)
    return diff <= 7 && diff >= 0
  })

  if (upcomingSchedules.length > 0) {
    alerts.push({
      type: "warning",
      message: `Có ${upcomingSchedules.length} lịch bảo trì sắp tới trong 7 ngày`,
    })
  }

  // Cảnh báo bảo hành sắp hết hạn
  const expiringWarranties = warranties.filter((w) => {
    const endDate = new Date(w.NgayKetThuc)
    const today = new Date()
    const diff = (endDate - today) / (1000 * 60 * 60 * 24)
    return diff <= 30 && diff >= 0
  })

  if (expiringWarranties.length > 0) {
    alerts.push({
      type: "warning",
      message: `Có ${expiringWarranties.length} bảo hành sắp hết hạn trong 30 ngày`,
    })
  }

  // Cảnh báo tồn kho thấp
  const lowStock = inventory.filter((i) => i.SoLuongTon <= i.SoLuongToiThieu)

  if (lowStock.length > 0) {
    alerts.push({
      type: "danger",
      message: `Có ${lowStock.length} linh kiện tồn kho thấp cần nhập thêm`,
    })
  }

  const alertList = document.getElementById("alertList")
  if (alerts.length === 0) {
    alertList.innerHTML = '<p class="no-alerts">Không có cảnh báo nào</p>'
  } else {
    alertList.innerHTML = alerts
      .map(
        (alert) => `
            <div class="alert alert-${alert.type}">
                <i class="icon-${alert.type}"></i>
                ${alert.message}
            </div>
        `,
      )
      .join("")
  }
}

// Tải dữ liệu khi trang load
document.addEventListener("DOMContentLoaded", loadDashboardStats)
