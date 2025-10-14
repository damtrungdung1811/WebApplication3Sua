// Data Storage and Management System
// Using localStorage for data persistence

// Initialize default data
function initializeData() {
  if (!localStorage.getItem("initialized")) {
    // Sample data matching SQL schema
    const initialData = {
      // Nhân viên
      NhanVien: [
        {
          MaNV: "NV001",
          HoTen: "Nguyễn Văn An",
          Email: "an.nv@company.com",
          SDT: "0901234567",
          ChucVu: "Kỹ thuật viên",
          TrangThai: "Đang làm việc",
        },
        {
          MaNV: "NV002",
          HoTen: "Trần Thị Bình",
          Email: "binh.tt@company.com",
          SDT: "0912345678",
          ChucVu: "Trưởng phòng",
          TrangThai: "Đang làm việc",
        },
        {
          MaNV: "NV003",
          HoTen: "Lê Văn Cường",
          Email: "cuong.lv@company.com",
          SDT: "0923456789",
          ChucVu: "Kỹ thuật viên",
          TrangThai: "Đang làm việc",
        },
      ],
      // Khách hàng
      KhachHang: [
        {
          MaKH: "KH001",
          TenKH: "Công ty TNHH ABC",
          NguoiLienHe: "Nguyễn Văn A",
          Email: "contact@abc.com",
          SDT: "0281234567",
          DiaChi: "123 Đường ABC, Q1, TP.HCM",
        },
        {
          MaKH: "KH002",
          TenKH: "Công ty CP XYZ",
          NguoiLienHe: "Trần Thị B",
          Email: "info@xyz.com",
          SDT: "0287654321",
          DiaChi: "456 Đường XYZ, Q3, TP.HCM",
        },
      ],
      // Tài sản
      TaiSan: [
        {
          MaTS: "TS001",
          TenTS: "Máy nén khí Atlas Copco",
          LoaiTS: "Máy móc",
          MaKH: "KH001",
          SoSerial: "AC2022001",
          NhaSanXuat: "Atlas Copco",
          Model: "GA75",
          NgayMua: "2022-01-15",
          GiaTri: 150000000,
          ViTri: "Nhà máy A",
          TrangThai: "Hoạt động",
          GhiChu: "",
        },
        {
          MaTS: "TS002",
          TenTS: "Hệ thống điều hòa trung tâm",
          LoaiTS: "Điện lạnh",
          MaKH: "KH001",
          SoSerial: "DK2021001",
          NhaSanXuat: "Daikin",
          Model: "VRV-IV",
          NgayMua: "2021-06-20",
          GiaTri: 200000000,
          ViTri: "Tòa nhà B",
          TrangThai: "Hoạt động",
          GhiChu: "",
        },
        {
          MaTS: "TS003",
          TenTS: "Máy phát điện Cummins",
          LoaiTS: "Điện",
          MaKH: "KH002",
          SoSerial: "CM2020001",
          NhaSanXuat: "Cummins",
          Model: "C500D5",
          NgayMua: "2020-03-10",
          GiaTri: 300000000,
          ViTri: "Khu phát điện",
          TrangThai: "Bảo trì",
          GhiChu: "Đang bảo trì",
        },
      ],
      // Bảo hành
      BaoHanh: [
        {
          MaBH: "BH001",
          MaTS: "TS001",
          NgayBatDau: "2022-01-15",
          NgayKetThuc: "2025-01-15",
          LoaiBH: "Toàn diện",
          NhaCungCap: "Atlas Copco Vietnam",
          DienGiai: "Bảo hành 3 năm",
          TrangThai: "Còn hạn",
        },
        {
          MaBH: "BH002",
          MaTS: "TS002",
          NgayBatDau: "2021-06-20",
          NgayKetThuc: "2024-06-20",
          LoaiBH: "Bộ phận",
          NhaCungCap: "Daikin Vietnam",
          DienGiai: "Bảo hành máy nén",
          TrangThai: "Sắp hết hạn",
        },
      ],
      // Phiếu công việc
      PhieuCongViec: [
        {
          MaPCV: "PCV001",
          MaTS: "TS001",
          LoaiCV: "Bảo trì định kỳ",
          MaNV: "NV001",
          NgayBatDau: "2024-01-10",
          NgayKetThuc: "2024-01-10",
          MucDoUuTien: "Trung bình",
          TrangThai: "Hoàn thành",
          ChiPhi: 2000000,
          MoTa: "Bảo trì định kỳ 3 tháng",
        },
        {
          MaPCV: "PCV002",
          MaTS: "TS003",
          LoaiCV: "Sửa chữa",
          MaNV: "NV002",
          NgayBatDau: "2024-01-15",
          NgayKetThuc: null,
          MucDoUuTien: "Cao",
          TrangThai: "Đang xử lý",
          ChiPhi: 5000000,
          MoTa: "Sửa chữa hệ thống làm mát",
        },
      ],
      // Lịch bảo trì
      LichBaoTri: [
        {
          MaLBT: "LBT001",
          MaTS: "TS001",
          LoaiBT: "Định kỳ",
          ChuKy: "Hàng tháng",
          NgayBTTiepTheo: "2024-02-15",
          MaNV: "NV001",
          TrangThai: "Đang hoạt động",
          GhiChu: "Kiểm tra máy nén",
        },
        {
          MaLBT: "LBT002",
          MaTS: "TS002",
          LoaiBT: "Định kỳ",
          ChuKy: "Hàng quý",
          NgayBTTiepTheo: "2024-03-20",
          MaNV: "NV002",
          TrangThai: "Đang hoạt động",
          GhiChu: "Vệ sinh điều hòa",
        },
      ],
      // Phiếu sự cố
      PhieuSuCo: [
        {
          MaPSC: "PSC001",
          MaTS: "TS003",
          MaNVBaoCao: "NV001",
          NgayBaoCao: "2024-01-15",
          MucDoNghiemTrong: "Cao",
          TrangThai: "Đang xử lý",
          MoTa: "Máy phát không khởi động",
          NguyenNhan: "Đang điều tra",
          BienPhapXuLy: "Đã cử kỹ thuật viên",
        },
        {
          MaPSC: "PSC002",
          MaTS: "TS001",
          MaNVBaoCao: "NV002",
          NgayBaoCao: "2024-01-05",
          MucDoNghiemTrong: "Trung bình",
          TrangThai: "Đã xử lý",
          MoTa: "Tiếng kêu bất thường",
          NguyenNhan: "Thiếu dầu",
          BienPhapXuLy: "Đã bổ sung dầu",
        },
      ],
      // Linh kiện
      LinhKien: [
        {
          MaLK: "LK001",
          TenLK: "Dầu máy nén Atlas Copco",
          DonViTinh: "Lít",
          SoLuongTon: 50,
          SoLuongToiThieu: 20,
          DonGia: 500000,
          NhaCungCap: "Atlas Copco Vietnam",
          TrangThai: "Còn hàng",
        },
        {
          MaLK: "LK002",
          TenLK: "Lọc gió máy nén",
          DonViTinh: "Cái",
          SoLuongTon: 8,
          SoLuongToiThieu: 10,
          DonGia: 1200000,
          NhaCungCap: "Atlas Copco Vietnam",
          TrangThai: "Sắp hết",
        },
        {
          MaLK: "LK003",
          TenLK: "Gas R410A",
          DonViTinh: "Kg",
          SoLuongTon: 30,
          SoLuongToiThieu: 15,
          DonGia: 800000,
          NhaCungCap: "Daikin Vietnam",
          TrangThai: "Còn hàng",
        },
      ],
      // Phiếu kho
      PhieuKho: [
        {
          MaPK: "PK001",
          LoaiPhieu: "Nhập",
          NgayLap: "2024-01-05",
          NguoiLap: "NV003",
          TongGiaTri: 25000000,
          TrangThai: "Đã duyệt",
          GhiChu: "Nhập hàng tháng 1",
        },
        {
          MaPK: "PK002",
          LoaiPhieu: "Xuất",
          NgayLap: "2024-01-10",
          NguoiLap: "NV001",
          TongGiaTri: 2000000,
          TrangThai: "Đã duyệt",
          GhiChu: "Xuất cho PCV001",
        },
      ],
      // Checklist
      Checklist: [
        { MaCL: "CL001", TenCL: "Bảo trì máy nén khí", LoaiThietBi: "Máy nén khí", MoTa: "Checklist bảo trì định kỳ" },
        {
          MaCL: "CL002",
          TenCL: "Bảo trì điều hòa",
          LoaiThietBi: "Điều hòa",
          MoTa: "Checklist bảo trì hệ thống điều hòa",
        },
      ],
      // Vai trò
      VaiTro: [
        { MaVT: "VT001", TenVT: "Admin", MoTa: "Quản trị viên hệ thống", SoNguoiDung: 2 },
        { MaVT: "VT002", TenVT: "Quản lý", MoTa: "Quản lý bảo trì", SoNguoiDung: 3 },
        { MaVT: "VT003", TenVT: "Nhân viên", MoTa: "Nhân viên kỹ thuật", SoNguoiDung: 8 },
      ],
      // Nhật ký hệ thống
      NhatKyHeThong: [
        {
          MaNK: "NK001",
          MaNV: "NV001",
          HanhDong: "Đăng nhập",
          MoTa: "Đăng nhập vào hệ thống",
          ThoiGian: "2024-01-15 08:30:00",
          DiaChi: "192.168.1.100",
        },
        {
          MaNK: "NK002",
          MaNV: "NV001",
          HanhDong: "Tạo phiếu",
          MoTa: "Tạo phiếu công việc PCV002",
          ThoiGian: "2024-01-15 09:15:00",
          DiaChi: "192.168.1.100",
        },
      ],
    }

    // Save to localStorage
    for (const key in initialData) {
      localStorage.setItem(key, JSON.stringify(initialData[key]))
    }
    localStorage.setItem("initialized", "true")
  }
}

// Get data from localStorage
function getData(tableName) {
  const data = localStorage.getItem(tableName)
  return data ? JSON.parse(data) : []
}

// Save data to localStorage
function saveData(tableName, data) {
  localStorage.setItem(tableName, JSON.stringify(data))
}

// Add new record
function addData(tableName, record) {
  const data = getData(tableName)
  data.push(record)
  saveData(tableName, data)
}

// Update record
function updateData(tableName, id, updatedRecord) {
  const data = getData(tableName)
  const idField = getIdField(tableName)
  const index = data.findIndex((item) => item[idField] === id)
  if (index !== -1) {
    data[index] = { ...data[index], ...updatedRecord }
    saveData(tableName, data)
    return true
  }
  return false
}

// Delete record
function deleteData(tableName, id) {
  const data = getData(tableName)
  const idField = getIdField(tableName)
  const filtered = data.filter((item) => item[idField] !== id)
  saveData(tableName, filtered)
}

// Get ID field name for each table
function getIdField(tableName) {
  const idFields = {
    NhanVien: "MaNV",
    KhachHang: "MaKH",
    TaiSan: "MaTS",
    BaoHanh: "MaBH",
    PhieuCongViec: "MaPCV",
    LichBaoTri: "MaLBT",
    PhieuSuCo: "MaPSC",
    LinhKien: "MaLK",
    PhieuKho: "MaPK",
    Checklist: "MaCL",
    VaiTro: "MaVT",
    NhatKyHeThong: "MaNK",
  }
  return idFields[tableName] || "id"
}

// Helper functions
function getCustomerName(maKH) {
  const customers = getData("KhachHang")
  const customer = customers.find((c) => c.MaKH === maKH)
  return customer ? customer.TenKH : "-"
}

function getEmployeeName(maNV) {
  const employees = getData("NhanVien")
  const employee = employees.find((e) => e.MaNV === maNV)
  return employee ? employee.HoTen : "-"
}

function getAssetName(maTS) {
  const assets = getData("TaiSan")
  const asset = assets.find((a) => a.MaTS === maTS)
  return asset ? asset.TenTS : "-"
}

// Initialize data on load
initializeData()
