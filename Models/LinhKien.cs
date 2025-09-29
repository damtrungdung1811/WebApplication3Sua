using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication3.Models
{
    [Table("LinhKien")]
    public class LinhKien
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaLinhKien { get; set; }   // IDENTITY, khóa chính

        [Required]
        [StringLength(200)]
        public string TenLinhKien { get; set; } = string.Empty;

        [StringLength(100)]
        public string? MaSo { get; set; }

        [Required]
        public int TonKho { get; set; } = 0;   // DEFAULT (0)
    }
}
