using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{
    public class NhatKyHeThong
    {
        [Key]
        public long MaLog { get; set; }

        [Required, MaxLength(50)]
        public string TenBang { get; set; }

        [Required]
        public int MaBanGhi { get; set; }

        [Required, MaxLength(50)]
        public string HanhDong { get; set; }

        public string GiaTriCu { get; set; }
        public string GiaTriMoi { get; set; }

        [MaxLength(200)]
        public string ThayDoiBoi { get; set; }

        public DateTime ThoiGian { get; set; } = DateTime.UtcNow;
    }
}
