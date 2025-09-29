using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{
    public class NhanVien
    {
        [Key]
        public int MaNV { get; set; }

        [Required]
        [MaxLength(200)]
        public string HoTen { get; set; }

        [MaxLength(20)]
        public string? SoDienThoai { get; set; }

        [MaxLength(100)]
        public string? Email { get; set; }

        [MaxLength(20)]
        public string TrangThai { get; set; }
    }
}
