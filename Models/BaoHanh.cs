using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication3.Models
{
    [Table("BaoHanh")]  // ánh xạ đúng tên bảng trong SQL
    public class BaoHanh
    {
        [Key]
        public int MaBaoHanh { get; set; }

        [Required]
        [StringLength(200)]
        public string NhaCungCap { get; set; } = string.Empty; // fix warning

        [Required]
        [DataType(DataType.Date)]
        public DateTime NgayBatDau { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime NgayKetThuc { get; set; }

        public string? DieuKhoan { get; set; }

        // Khóa ngoại đến Nhân Viên
        public int? MaNV { get; set; }

        [ForeignKey("MaNV")]
        public NhanVien? NhanVien { get; set; }
    }
}
