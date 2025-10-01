using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication3.Models
{
    public class ChecklistItem
    {
        [Key]
        public int ItemID { get; set; }

        [Required]
        public int ChecklistID { get; set; }

        [ForeignKey(nameof(ChecklistID))]
        public Checklist Checklist { get; set; }

        [Required, MaxLength(500)]
        public string NoiDung { get; set; }
        public ICollection<PCV_Checklist> PCV_Checklists { get; set; }
    }
}
