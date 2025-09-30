using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication3.Models
{
    public class PhieuSuCo
    {
        [Key]
        public int MaSuCo { get; set; }

        [Required]
        public int MaTaiSan { get; set; }
        [ForeignKey("MaTaiSan")]
        public TaiSan TaiSan { get; set; }

        public int? MaKH { get; set; }
        [ForeignKey("MaKH")]
        public KhachHang? KhachHang { get; set; }

        [Required]
        [StringLength(200)]
        public string NguoiBao { get; set; }

        [Required]
        [StringLength(20)]
        public string MucUuTien { get; set; } = "Trung bình";

        public int? SLA_Gio { get; set; }

        public int? MaNV_TiepNhan { get; set; }
        [ForeignKey("MaNV_TiepNhan")]
        public NhanVien? NhanVien { get; set; }

        public DateTime NgayTao { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(30)]
        public string TrangThai { get; set; } = "Mới";

        public string? MoTa { get; set; }

        public int? MaPhieuCV { get; set; }
        [ForeignKey("MaPhieuCV")]
        public PhieuCongViec? PhieuCongViec { get; set; }
    }
}
