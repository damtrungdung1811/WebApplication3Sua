using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication3.Models
{
    public class TaiSan
    {
        [Key]
        public int MaTaiSan { get; set; }

        [Required]
        [StringLength(200)]
        public string TenTaiSan { get; set; } = string.Empty;

        [StringLength(200)]
        public string? ViTri { get; set; }

        [DataType(DataType.Date)]
        public DateTime? NgayMua { get; set; }

        // Khóa ngoại đến BaoHanh
        public int? MaBaoHanh { get; set; }
        [ForeignKey("MaBaoHanh")]
        public BaoHanh? BaoHanh { get; set; }

        // Khóa ngoại đến KhachHang
        public int? MaKH { get; set; }
        [ForeignKey("MaKH")]
        public KhachHang? KhachHang { get; set; }

        [Required]
        [StringLength(50)]
        public string TrangThai { get; set; } = "Đang hoạt động";

        public string? GhiChu { get; set; }
    }
}
