using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebApplication3.Models
{
    [Table("ChecklistItem")]
    public class ChecklistItem
    {
        [Key]
        public int ItemID { get; set; }

        // Cho phép null để EF tự gán khi thêm Checklist cha
        public int? ChecklistID { get; set; }

        [ForeignKey(nameof(ChecklistID))]
        [JsonIgnore] // Tránh vòng lặp khi serialize
        public Checklist? Checklist { get; set; }

        [Required, MaxLength(500)]
        public string NoiDung { get; set; } = string.Empty;

        [JsonIgnore]
        public ICollection<PCV_Checklist>? PCV_Checklists { get; set; }
    }
}
