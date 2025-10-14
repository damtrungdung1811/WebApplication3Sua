using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication3.Models
{
    public class PhieuSuCo
    {
        [Key]
        public int MaSuCo { get; set; }

        // 🔹 Tài sản bị sự cố
        [Required]
        public int MaTaiSan { get; set; }
        [ForeignKey("MaTaiSan")]
        public TaiSan? TaiSan { get; set; }   // ✅ Cho phép null để tránh lỗi ModelState

        // 🔹 Khách hàng liên quan (có thể null nếu nội bộ)
        public int? MaKH { get; set; }
        [ForeignKey("MaKH")]
        public KhachHang? KhachHang { get; set; }

        // 🔹 Người báo sự cố
        [StringLength(200)]
        public string? NguoiBao { get; set; }  // ✅ Cho phép null hoặc không bắt buộc khi POST

        // 🔹 Mức độ ưu tiên
        [Required]
        [StringLength(20)]
        public string MucUuTien { get; set; } = "Trung bình";

        // 🔹 Thời gian xử lý theo SLA
        public int? SLA_Gio { get; set; }

        // 🔹 Nhân viên tiếp nhận
        public int? MaNV_TiepNhan { get; set; }
        [ForeignKey("MaNV_TiepNhan")]
        public NhanVien? NhanVien { get; set; }

        // 🔹 Ngày tạo phiếu
        public DateTime NgayTao { get; set; } = DateTime.UtcNow;

        // 🔹 Trạng thái
        [Required]
        [StringLength(30)]
        public string TrangThai { get; set; } = "Mới";

        // 🔹 Mô tả chi tiết sự cố
        public string? MoTa { get; set; }

        // 🔹 Phiếu công việc liên quan (nếu có)
        public int? MaPhieuCV { get; set; }
        [ForeignKey("MaPhieuCV")]
        public PhieuCongViec? PhieuCongViec { get; set; }
    }
}
