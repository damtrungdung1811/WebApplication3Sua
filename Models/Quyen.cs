using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{
    public class Quyen
    {
        [Key]
        public int QuyenID { get; set; }

        [Required]
        [MaxLength(100)]
        public string TenQuyen { get; set; }

        [MaxLength(50)]
        public string Nhom { get; set; }

        [MaxLength(200)]
        public string MoTa { get; set; }
    }
}
