if (!localStorage.getItem("currentUser")) {
  window.location.href = "index.html"
}

let warranties = []
let filteredWarranties = []
let editingWarrantyId = null

function loadWarranties() {
  warranties = JSON.parse(localStorage.getItem("warranties") || "[]")
  filteredWarranties = [...warranties]
  renderWarranties()
  loadAssetOptions()
}

function getWarrantyStatus(endDate) {
  const today = new Date()
  const end = new Date(endDate)
  const diffDays = Math.ceil((end - today) / (1000 * 60 * 60 * 24))

  if (diffDays < 0) return "Hết hạn"
  if (diffDays <= 30) return "Sắp hết hạn"
  return "Còn hạn"
}

function renderWarranties() {
  const tbody = document.getElementById("warrantiesTableBody")

  if (filteredWarranties.length === 0) {
    tbody.innerHTML = '<tr><td colspan="7" style="text-align: center; padding: 2rem;">Không có dữ liệu</td></tr>'
    return
  }

  tbody.innerHTML = filteredWarranties
    .map((warranty) => {
      const status = getWarrantyStatus(warranty.NgayKetThuc)
      return `
            <tr>
                <td>${warranty.MaBaoHanh}</td>
                <td>${warranty.TenTaiSan || warranty.MaTaiSan}</td>
                <td>${warranty.NhaCungCap}</td>
                <td>${new Date(warranty.NgayBatDau).toLocaleDateString("vi-VN")}</td>
                <td>${new Date(warranty.NgayKetThuc).toLocaleDateString("vi-VN")}</td>
                <td><span class="badge badge-${status.toLowerCase().replace(" ", "-")}">${status}</span></td>
                <td>
                    <button class="btn btn-sm btn-primary" onclick="viewWarranty('${warranty.MaBaoHanh}')">Xem</button>
                    <button class="btn btn-sm btn-warning" onclick="editWarranty('${warranty.MaBaoHanh}')">Sửa</button>
                    <button class="btn btn-sm btn-danger" onclick="deleteWarranty('${warranty.MaBaoHanh}')">Xóa</button>
                </td>
            </tr>
        `
    })
    .join("")
}

function loadAssetOptions() {
  const assets = JSON.parse(localStorage.getItem("assets") || "[]")
  const select = document.getElementById("assetId")
  select.innerHTML =
    '<option value="">Chọn tài sản</option>' +
    assets.map((asset) => `<option value="${asset.MaTaiSan}">${asset.TenTaiSan}</option>`).join("")
}

document.getElementById("searchInput").addEventListener("input", (e) => {
  const searchTerm = e.target.value.toLowerCase()
  filteredWarranties = warranties.filter(
    (w) => w.MaBaoHanh.toLowerCase().includes(searchTerm) || w.NhaCungCap.toLowerCase().includes(searchTerm),
  )
  renderWarranties()
})

document.getElementById("statusFilter").addEventListener("change", (e) => {
  const status = e.target.value
  if (status === "all") {
    filteredWarranties = [...warranties]
  } else {
    filteredWarranties = warranties.filter((w) => getWarrantyStatus(w.NgayKetThuc) === status)
  }
  renderWarranties()
})

document.getElementById("addWarrantyBtn").addEventListener("click", () => {
  editingWarrantyId = null
  document.getElementById("warrantyForm").reset()
  document.getElementById("modalTitle").textContent = "Thêm bảo hành mới"
  document.getElementById("warrantyModal").style.display = "flex"
})

document.getElementById("closeModal").addEventListener("click", () => {
  document.getElementById("warrantyModal").style.display = "none"
})

document.getElementById("warrantyForm").addEventListener("submit", (e) => {
  e.preventDefault()

  const assetId = document.getElementById("assetId").value
  const assets = JSON.parse(localStorage.getItem("assets") || "[]")
  const asset = assets.find((a) => a.MaTaiSan === assetId)

  const formData = {
    MaBaoHanh: editingWarrantyId || "BH" + Date.now(),
    MaTaiSan: assetId,
    TenTaiSan: asset ? asset.TenTaiSan : "",
    NhaCungCap: document.getElementById("supplier").value,
    NgayBatDau: document.getElementById("startDate").value,
    NgayKetThuc: document.getElementById("endDate").value,
    DieuKhoan: document.getElementById("terms").value,
  }

  if (editingWarrantyId) {
    const index = warranties.findIndex((w) => w.MaBaoHanh === editingWarrantyId)
    warranties[index] = formData
  } else {
    warranties.push(formData)
  }

  localStorage.setItem("warranties", JSON.stringify(warranties))
  loadWarranties()
  document.getElementById("warrantyModal").style.display = "none"
})

function viewWarranty(id) {
  const warranty = warranties.find((w) => w.MaBaoHanh === id)
  if (warranty) {
    const status = getWarrantyStatus(warranty.NgayKetThuc)
    alert(
      `Thông tin bảo hành:\n\nMã: ${warranty.MaBaoHanh}\nTài sản: ${warranty.TenTaiSan}\nNhà cung cấp: ${warranty.NhaCungCap}\nTừ: ${new Date(warranty.NgayBatDau).toLocaleDateString("vi-VN")}\nĐến: ${new Date(warranty.NgayKetThuc).toLocaleDateString("vi-VN")}\nTrạng thái: ${status}\nĐiều khoản: ${warranty.DieuKhoan || "Không có"}`,
    )
  }
}

function editWarranty(id) {
  const warranty = warranties.find((w) => w.MaBaoHanh === id)
  if (warranty) {
    editingWarrantyId = id
    document.getElementById("assetId").value = warranty.MaTaiSan
    document.getElementById("supplier").value = warranty.NhaCungCap
    document.getElementById("startDate").value = warranty.NgayBatDau
    document.getElementById("endDate").value = warranty.NgayKetThuc
    document.getElementById("terms").value = warranty.DieuKhoan || ""
    document.getElementById("modalTitle").textContent = "Chỉnh sửa bảo hành"
    document.getElementById("warrantyModal").style.display = "flex"
  }
}

function deleteWarranty(id) {
  if (confirm("Bạn có chắc chắn muốn xóa bảo hành này?")) {
    warranties = warranties.filter((w) => w.MaBaoHanh !== id)
    localStorage.setItem("warranties", JSON.stringify(warranties))
    loadWarranties()
  }
}

document.addEventListener("DOMContentLoaded", loadWarranties)
