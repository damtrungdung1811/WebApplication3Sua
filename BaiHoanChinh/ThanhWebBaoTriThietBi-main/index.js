// Xử lý đăng nhập
document.addEventListener("DOMContentLoaded", () => {
  const loginForm = document.getElementById("loginForm")

  if (loginForm) {
    loginForm.addEventListener("submit", (e) => {
      e.preventDefault()

      const username = document.getElementById("username").value
      const password = document.getElementById("password").value
      const errorDiv = document.getElementById("error")

      // Kiểm tra đăng nhập
      const users = JSON.parse(localStorage.getItem("users") || "[]")
      const user = users.find((u) => u.TenDangNhap === username && u.MatKhau === password)

      if (user || (username === "admin" && password === "admin123")) {
        // Lưu thông tin đăng nhập
        const userData = user || {
          MaNguoiDung: 1,
          TenDangNhap: "admin",
          HoTen: "Quản trị viên",
          VaiTro: "Admin",
        }
        localStorage.setItem("currentUser", JSON.stringify(userData))
        window.location.href = "dashboard.html"
      } else {
        errorDiv.textContent = "Tên đăng nhập hoặc mật khẩu không đúng!"
        errorDiv.style.display = "block"
      }
    })
  }
})
