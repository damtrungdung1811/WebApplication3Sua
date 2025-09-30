using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication3.Models
{
    public class NguoiDung
    {
        [Key]
        public int MaNguoiDung { get; set; }

        [Required]
        [MaxLength(100)]
        public string TenDangNhap { get; set; }

        [Required]
        [MaxLength(500)]
        public string MatKhauHash { get; set; }

        // ✅ Cho phép null để khớp với DB
        [MaxLength(200)]
        public string? Email { get; set; }

        // Quan hệ với NhanVien (có thể null)
        public int? MaNV { get; set; }

        [ForeignKey("MaNV")]
        public NhanVien? NhanVien { get; set; }

        // Quan hệ với VaiTro (bắt buộc)
        public int VaiTroID { get; set; }

        [ForeignKey("VaiTroID")]
        public VaiTro VaiTro { get; set; }

        [Required]
        [MaxLength(20)]
        public string TrangThai { get; set; } = "Hoạt động";

        public DateTime NgayTao { get; set; } = DateTime.UtcNow;

        public DateTime? LanDangNhapCuoi { get; set; }
    }
}
