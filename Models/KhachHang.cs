using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{
    public class KhachHang
    {
        [Key]
        public int MaKH { get; set; }

        [Required]
        [MaxLength(200)]
        public string TenKH { get; set; }

        [MaxLength(200)]
        public string? Email { get; set; }

        [MaxLength(50)]
        public string? DienThoai { get; set; }

        [MaxLength(255)]
        public string? DiaChi { get; set; }
    }
}
