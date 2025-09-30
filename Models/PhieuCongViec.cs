using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication3.Models
{
    public class PhieuCongViec
    {
        [Key]
        public int MaPhieuCV { get; set; }

        [Required]
        [StringLength(10)]
        public string Loai { get; set; }  // PM / CM

        public int? MaLich { get; set; }
        [ForeignKey("MaLich")]
        public LichBaoTri? LichBaoTri { get; set; }

        [Required]
        public int MaTaiSan { get; set; }
        [ForeignKey("MaTaiSan")]
        public TaiSan? TaiSan { get; set; }


        [Required]
        [StringLength(300)]
        public string TieuDe { get; set; }

        [Required]
        [StringLength(20)]
        public string MucUuTien { get; set; } = "Trung bình";

        public int? SLA_Gio { get; set; }

        public string? MoTa { get; set; }

        public int? MaNV_PhanCong { get; set; }
        [ForeignKey("MaNV_PhanCong")]
        public NhanVien? NhanVien { get; set; }

        public DateTime NgayTao { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(30)]
        public string TrangThai { get; set; } = "Mới";

        public DateTime? NgayBatDau { get; set; }
        public DateTime? NgayHoanThanh { get; set; }

        public string? GhiChu { get; set; }
    }
}
