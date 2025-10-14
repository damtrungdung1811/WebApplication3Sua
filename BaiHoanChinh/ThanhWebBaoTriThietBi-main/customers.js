// Kiểm tra đăng nhập
if (!localStorage.getItem("currentUser")) {
  window.location.href = "index.html"
}

let customers = []
let filteredCustomers = []
let editingCustomerId = null

function loadCustomers() {
  customers = JSON.parse(localStorage.getItem("customers") || "[]")
  filteredCustomers = [...customers]
  renderCustomers()
}

function renderCustomers() {
  const tbody = document.getElementById("customersTableBody")

  if (filteredCustomers.length === 0) {
    tbody.innerHTML = '<tr><td colspan="6" style="text-align: center; padding: 2rem;">Không có dữ liệu</td></tr>'
    return
  }

  tbody.innerHTML = filteredCustomers
    .map(
      (customer) => `
        <tr>
            <td>${customer.MaKhachHang}</td>
            <td>${customer.TenKhachHang}</td>
            <td>${customer.SoDienThoai}</td>
            <td>${customer.Email}</td>
            <td>${customer.DiaChi}</td>
            <td>
                <button class="btn btn-sm btn-primary" onclick="viewCustomer('${customer.MaKhachHang}')">Xem</button>
                <button class="btn btn-sm btn-warning" onclick="editCustomer('${customer.MaKhachHang}')">Sửa</button>
                <button class="btn btn-sm btn-danger" onclick="deleteCustomer('${customer.MaKhachHang}')">Xóa</button>
            </td>
        </tr>
    `,
    )
    .join("")
}

document.getElementById("searchInput").addEventListener("input", (e) => {
  const searchTerm = e.target.value.toLowerCase()
  filteredCustomers = customers.filter(
    (customer) =>
      customer.TenKhachHang.toLowerCase().includes(searchTerm) ||
      customer.MaKhachHang.toLowerCase().includes(searchTerm) ||
      customer.SoDienThoai.includes(searchTerm),
  )
  renderCustomers()
})

document.getElementById("addCustomerBtn").addEventListener("click", () => {
  editingCustomerId = null
  document.getElementById("customerForm").reset()
  document.getElementById("modalTitle").textContent = "Thêm khách hàng mới"
  document.getElementById("customerModal").style.display = "flex"
})

document.getElementById("closeModal").addEventListener("click", () => {
  document.getElementById("customerModal").style.display = "none"
})

document.getElementById("customerForm").addEventListener("submit", (e) => {
  e.preventDefault()

  const formData = {
    MaKhachHang: editingCustomerId || "KH" + Date.now(),
    TenKhachHang: document.getElementById("customerName").value,
    SoDienThoai: document.getElementById("customerPhone").value,
    Email: document.getElementById("customerEmail").value,
    DiaChi: document.getElementById("customerAddress").value,
    LoaiKhachHang: document.getElementById("customerType").value,
  }

  if (editingCustomerId) {
    const index = customers.findIndex((c) => c.MaKhachHang === editingCustomerId)
    customers[index] = formData
  } else {
    customers.push(formData)
  }

  localStorage.setItem("customers", JSON.stringify(customers))
  loadCustomers()
  document.getElementById("customerModal").style.display = "none"
})

function viewCustomer(id) {
  const customer = customers.find((c) => c.MaKhachHang === id)
  if (customer) {
    alert(
      `Thông tin khách hàng:\n\nMã: ${customer.MaKhachHang}\nTên: ${customer.TenKhachHang}\nSĐT: ${customer.SoDienThoai}\nEmail: ${customer.Email}\nĐịa chỉ: ${customer.DiaChi}\nLoại: ${customer.LoaiKhachHang}`,
    )
  }
}

function editCustomer(id) {
  const customer = customers.find((c) => c.MaKhachHang === id)
  if (customer) {
    editingCustomerId = id
    document.getElementById("customerName").value = customer.TenKhachHang
    document.getElementById("customerPhone").value = customer.SoDienThoai
    document.getElementById("customerEmail").value = customer.Email
    document.getElementById("customerAddress").value = customer.DiaChi
    document.getElementById("customerType").value = customer.LoaiKhachHang
    document.getElementById("modalTitle").textContent = "Chỉnh sửa khách hàng"
    document.getElementById("customerModal").style.display = "flex"
  }
}

function deleteCustomer(id) {
  if (confirm("Bạn có chắc chắn muốn xóa khách hàng này?")) {
    customers = customers.filter((c) => c.MaKhachHang !== id)
    localStorage.setItem("customers", JSON.stringify(customers))
    loadCustomers()
  }
}

document.addEventListener("DOMContentLoaded", loadCustomers)
