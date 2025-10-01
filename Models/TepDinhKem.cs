using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{
    public class TepDinhKem
    {
        [Key]
        public int MaTep { get; set; }

        [Required, MaxLength(50)]
        public string BangLienQuan { get; set; }

        [Required]
        public int MaLienQuan { get; set; }

        [Required, MaxLength(255)]
        public string TenTep { get; set; }

        [Required, MaxLength(500)]
        public string DuongDan { get; set; }

        public DateTime NgayTaiLen { get; set; } = DateTime.UtcNow;
    }
}
