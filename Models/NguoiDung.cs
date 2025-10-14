using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication3.Models
{
    [Table("NguoiDung")]
    public class NguoiDung
    {
        [Key]
        [Column("MaNguoiDung")]
        public int MaNguoiDung { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("TenDangNhap")]
        public string TenDangNhap { get; set; }

        [Required]
        [MaxLength(500)]
        [Column("MatKhauHash")]
        public string MatKhauHash { get; set; }

        [MaxLength(200)]
        [Column("Email")]
        public string? Email { get; set; }

        // 🔹 Liên kết với bảng NhanVien (có thể null)
        [Column("MaNV")]
        public int? MaNV { get; set; }

        [ForeignKey("MaNV")]
        public NhanVien? NhanVien { get; set; }

        // 🔹 Liên kết với VaiTro — chỉ cần ID, KHÔNG ép VaiTro phải có (nullable)
        [Required]
        [Column("VaiTroID")]
        public int VaiTroID { get; set; }

        [ForeignKey("VaiTroID")]
        public VaiTro? VaiTro { get; set; }   // ✅ cho phép nullable để tránh lỗi khi POST

        [Required]
        [MaxLength(20)]
        [Column("TrangThai")]
        public string TrangThai { get; set; } = "Hoạt động";

        [Column("NgayTao")]
        public DateTime NgayTao { get; set; } = DateTime.UtcNow;

        [Column("LanDangNhapCuoi")]
        public DateTime? LanDangNhapCuoi { get; set; }
    }
}
