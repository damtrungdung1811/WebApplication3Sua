using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication3.Models
{
    public class PCV_Checklist
    {
        [Key]
        public int ID { get; set; }
        public int MaPhieuCV { get; set; }
        public int ItemID { get; set; }
        public bool DaHoanThanh { get; set; }

        // 🔹 Thêm thuộc tính này để EF hiểu quan hệ
        [ForeignKey("ItemID")]
        public ChecklistItem Item { get; set; }
    }
}
