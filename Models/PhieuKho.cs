using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{
    public class PhieuKho
    {
        [Key]
        public int MaPhieuKho { get; set; }

        [Required]
        [MaxLength(10)]
        public string Loai { get; set; } = "";  // "Nhap" hoặc "Xuat"

        // 🟢 Đổi lại đúng tên cột có thật trong SQL
        public DateTime NgayLap { get; set; } = DateTime.UtcNow;

        [MaxLength(500)]
        public string? GhiChu { get; set; }
    }
}
