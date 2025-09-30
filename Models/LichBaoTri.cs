using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication3.Models
{
    public class LichBaoTri
    {
        [Key]
        public int MaLich { get; set; }

        [Required]
        public int MaTaiSan { get; set; }
        [ForeignKey("MaTaiSan")]
        public TaiSan TaiSan { get; set; }

        public int? MaNV { get; set; }
        [ForeignKey("MaNV")]
        public NhanVien? NhanVien { get; set; }

        [Required]
        [StringLength(50)]
        public string TanSuat { get; set; }

        public int? SoNgayLapLai { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime NgayKeTiep { get; set; }

        public string? ChecklistMacDinh { get; set; }

        public bool HieuLuc { get; set; } = true;
    }
}
