using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication3.Models
{
    [Table("PhieuKho_ChiTiet")] // ✅ khớp tên bảng SQL
    public class PhieuKhoChiTiet
    {
        [Key]
        [Column("MaCT")]
        public int MaCT { get; set; }

        // ===== Khóa ngoại tới PHIEUKHO =====
        [Required]
        [Column("MaPhieuKho")]
        public int MaPhieuKho { get; set; }

        [ForeignKey("MaPhieuKho")]
        public PhieuKho? PhieuKho { get; set; }

        // ===== Khóa ngoại tới LINHKIEN =====
        [Required]
        [Column("MaLinhKien")]
        public int MaLinhKien { get; set; }

        [ForeignKey("MaLinhKien")]
        public LinhKien? LinhKien { get; set; }

        // ===== SỐ LƯỢNG & ĐƠN GIÁ =====
        [Required]
        [Column("SoLuong")]
        public int SoLuong { get; set; }

        [Column("DonGia", TypeName = "decimal(18,2)")]
        public decimal? DonGia { get; set; }
    }
}
