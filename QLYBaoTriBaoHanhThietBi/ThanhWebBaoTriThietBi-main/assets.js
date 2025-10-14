// Kiểm tra đăng nhập
if (!localStorage.getItem("currentUser")) {
  window.location.href = "index.html"
}

let assets = []
let filteredAssets = []
let editingAssetId = null

// Tải dữ liệu
function loadAssets() {
  assets = JSON.parse(localStorage.getItem("assets") || "[]")
  filteredAssets = [...assets]
  renderAssets()
}

// Hiển thị danh sách tài sản
function renderAssets() {
  const tbody = document.getElementById("assetsTableBody")

  if (filteredAssets.length === 0) {
    tbody.innerHTML = '<tr><td colspan="7" style="text-align: center; padding: 2rem;">Không có dữ liệu</td></tr>'
    return
  }

  tbody.innerHTML = filteredAssets
    .map(
      (asset) => `
        <tr>
            <td>${asset.MaTaiSan}</td>
            <td>${asset.TenTaiSan}</td>
            <td>${asset.LoaiTaiSan}</td>
            <td>${asset.ViTri}</td>
            <td><span class="badge badge-${asset.TrangThai.toLowerCase().replace(" ", "-")}">${asset.TrangThai}</span></td>
            <td>${new Date(asset.NgayMua).toLocaleDateString("vi-VN")}</td>
            <td>
                <button class="btn btn-sm btn-primary" onclick="viewAsset('${asset.MaTaiSan}')">Xem</button>
                <button class="btn btn-sm btn-warning" onclick="editAsset('${asset.MaTaiSan}')">Sửa</button>
                <button class="btn btn-sm btn-danger" onclick="deleteAsset('${asset.MaTaiSan}')">Xóa</button>
            </td>
        </tr>
    `,
    )
    .join("")
}

// Tìm kiếm
document.getElementById("searchInput").addEventListener("input", (e) => {
  const searchTerm = e.target.value.toLowerCase()
  filteredAssets = assets.filter(
    (asset) =>
      asset.TenTaiSan.toLowerCase().includes(searchTerm) ||
      asset.MaTaiSan.toLowerCase().includes(searchTerm) ||
      asset.LoaiTaiSan.toLowerCase().includes(searchTerm),
  )
  renderAssets()
})

// Lọc theo trạng thái
document.getElementById("statusFilter").addEventListener("change", (e) => {
  const status = e.target.value
  if (status === "all") {
    filteredAssets = [...assets]
  } else {
    filteredAssets = assets.filter((asset) => asset.TrangThai === status)
  }
  renderAssets()
})

// Thêm tài sản mới
document.getElementById("addAssetBtn").addEventListener("click", () => {
  editingAssetId = null
  document.getElementById("assetForm").reset()
  document.getElementById("modalTitle").textContent = "Thêm tài sản mới"
  document.getElementById("assetModal").style.display = "flex"
})

// Đóng modal
document.getElementById("closeModal").addEventListener("click", () => {
  document.getElementById("assetModal").style.display = "none"
})

// Lưu tài sản
document.getElementById("assetForm").addEventListener("submit", (e) => {
  e.preventDefault()

  const formData = {
    MaTaiSan: editingAssetId || "TS" + Date.now(),
    TenTaiSan: document.getElementById("assetName").value,
    LoaiTaiSan: document.getElementById("assetType").value,
    MoTa: document.getElementById("assetDescription").value,
    ViTri: document.getElementById("assetLocation").value,
    TrangThai: document.getElementById("assetStatus").value,
    NgayMua: document.getElementById("purchaseDate").value,
    GiaTri: Number.parseFloat(document.getElementById("assetValue").value),
    MaKhachHang: document.getElementById("customerId").value,
  }

  if (editingAssetId) {
    // Cập nhật
    const index = assets.findIndex((a) => a.MaTaiSan === editingAssetId)
    assets[index] = formData
  } else {
    // Thêm mới
    assets.push(formData)
  }

  localStorage.setItem("assets", JSON.stringify(assets))
  loadAssets()
  document.getElementById("assetModal").style.display = "none"
})

// Xem chi tiết
function viewAsset(id) {
  const asset = assets.find((a) => a.MaTaiSan === id)
  if (asset) {
    alert(
      `Thông tin tài sản:\n\nMã: ${asset.MaTaiSan}\nTên: ${asset.TenTaiSan}\nLoại: ${asset.LoaiTaiSan}\nVị trí: ${asset.ViTri}\nTrạng thái: ${asset.TrangThai}\nGiá trị: ${asset.GiaTri.toLocaleString("vi-VN")} VNĐ`,
    )
  }
}

// Sửa tài sản
function editAsset(id) {
  const asset = assets.find((a) => a.MaTaiSan === id)
  if (asset) {
    editingAssetId = id
    document.getElementById("assetName").value = asset.TenTaiSan
    document.getElementById("assetType").value = asset.LoaiTaiSan
    document.getElementById("assetDescription").value = asset.MoTa
    document.getElementById("assetLocation").value = asset.ViTri
    document.getElementById("assetStatus").value = asset.TrangThai
    document.getElementById("purchaseDate").value = asset.NgayMua
    document.getElementById("assetValue").value = asset.GiaTri
    document.getElementById("customerId").value = asset.MaKhachHang
    document.getElementById("modalTitle").textContent = "Chỉnh sửa tài sản"
    document.getElementById("assetModal").style.display = "flex"
  }
}

// Xóa tài sản
function deleteAsset(id) {
  if (confirm("Bạn có chắc chắn muốn xóa tài sản này?")) {
    assets = assets.filter((a) => a.MaTaiSan !== id)
    localStorage.setItem("assets", JSON.stringify(assets))
    loadAssets()
  }
}

// Tải dữ liệu khi trang load
document.addEventListener("DOMContentLoaded", loadAssets)
