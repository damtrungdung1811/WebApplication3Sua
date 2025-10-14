// Authentication management
class AuthManager {
  constructor() {
    this.currentUser = this.getCurrentUser()
  }

  login(username, password) {
    // Demo authentication
    const users = {
      admin: { username: "admin", password: "admin123", role: "Admin", name: "Quản trị viên" },
      manager: { username: "manager", password: "manager123", role: "Quản lý", name: "Nguyễn Văn A" },
      staff: { username: "staff", password: "staff123", role: "Nhân viên", name: "Trần Thị B" },
    }

    const user = users[username]
    if (user && user.password === password) {
      const session = {
        username: user.username,
        role: user.role,
        name: user.name,
        loginTime: new Date().toISOString(),
      }
      localStorage.setItem("currentUser", JSON.stringify(session))
      return { success: true, user: session }
    }
    return { success: false, message: "Tên đăng nhập hoặc mật khẩu không đúng" }
  }

  logout() {
    localStorage.removeItem("currentUser")
    window.location.href = "index.html"
  }

  getCurrentUser() {
    const userStr = localStorage.getItem("currentUser")
    return userStr ? JSON.parse(userStr) : null
  }

  isAuthenticated() {
    return this.getCurrentUser() !== null
  }

  checkAuth() {
    if (!this.isAuthenticated()) {
      window.location.href = "index.html"
    }
  }
}

// Create global instance
const authManager = new AuthManager()

// Login form handler
if (document.getElementById("loginForm")) {
  document.getElementById("loginForm").addEventListener("submit", (e) => {
    e.preventDefault()

    const username = document.getElementById("username").value
    const password = document.getElementById("password").value
    const errorMessage = document.getElementById("errorMessage")

    const result = authManager.login(username, password)

    if (result.success) {
      window.location.href = "dashboard.html"
    } else {
      errorMessage.textContent = result.message
      errorMessage.classList.remove("hidden")
    }
  })
}
