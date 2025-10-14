using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebApplication3.Models
{
    [Table("LichBaoTri")]
    public class LichBaoTri
    {
        [Key]
        [Column("MaLich")]
        public int MaLich { get; set; }

        // ------------------- KHÓA NGOẠI TÀI SẢN -------------------
        [Required(ErrorMessage = "Mã tài sản là bắt buộc.")]
        [Column("MaTaiSan")]
        public int MaTaiSan { get; set; }

        [ForeignKey(nameof(MaTaiSan))]
        [JsonIgnore] // tránh yêu cầu object TaiSan trong JSON khi POST
        public TaiSan? TaiSan { get; set; }

        // ------------------- KHÓA NGOẠI NHÂN VIÊN -------------------
        [Column("MaNV")]
        public int? MaNV { get; set; }

        [ForeignKey(nameof(MaNV))]
        [JsonIgnore] // tránh lỗi required khi không gửi object NhanVien
        public NhanVien? NhanVien { get; set; }

        // ------------------- CÁC THUỘC TÍNH CHÍNH -------------------
        [Required(ErrorMessage = "Tần suất không được để trống.")]
        [StringLength(50)]
        [Column("TanSuat")]
        public string TanSuat { get; set; } = string.Empty;

        [Column("SoNgayLapLai")]
        public int? SoNgayLapLai { get; set; }

        [Required(ErrorMessage = "Ngày kế tiếp là bắt buộc.")]
        [DataType(DataType.Date)]
        [Column("NgayKeTiep")]
        public DateTime NgayKeTiep { get; set; }

        [Column("ChecklistMacDinh")]
        [StringLength(500)]
        public string? ChecklistMacDinh { get; set; }

        [Column("HieuLuc")]
        public bool HieuLuc { get; set; } = true;
    }
}
