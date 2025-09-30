using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication3.Models
{
    public class PhieuKho_ChiTiet
    {
        [Key]
        public int MaCT { get; set; }

        // Khóa ngoại tới PhieuKho
        public int MaPhieuKho { get; set; }

        [ForeignKey("MaPhieuKho")]
        public PhieuKho? PhieuKho { get; set; }   // Cho phép nullable để tránh lỗi required

        // Khóa ngoại tới LinhKien
        public int MaLinhKien { get; set; }

        [ForeignKey("MaLinhKien")]
        public LinhKien? LinhKien { get; set; }   // Cho phép nullable để tránh lỗi required

        [Required]
        public int SoLuong { get; set; }

        [Required]
        public decimal DonGia { get; set; }
    }
}
