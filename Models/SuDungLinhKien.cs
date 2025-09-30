using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication3.Models
{
    public class SuDungLinhKien
    {
        [Key]
        public int MaSuDung { get; set; }

        // FK đến Phiếu Công Việc
        public int MaPhieuCV { get; set; }
        [ForeignKey("MaPhieuCV")]
        public PhieuCongViec? PhieuCongViec { get; set; }   // nullable

        // FK đến Linh Kiện
        public int MaLinhKien { get; set; }
        [ForeignKey("MaLinhKien")]
        public LinhKien? LinhKien { get; set; }   // nullable

        [Required]
        public int SoLuong { get; set; }

        [Required]
        public decimal DonGia { get; set; }

        [NotMapped]
        public decimal ThanhTien => SoLuong * DonGia;
    }
}
