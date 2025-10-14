using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication3.Models
{
    [Table("Checklist")]
    public class Checklist
    {
        [Key]
        public int ChecklistID { get; set; }

        [Required, MaxLength(200)]
        public string Ten { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? MoTa { get; set; }

        // Quan hệ 1 - N
        public ICollection<ChecklistItem>? Items { get; set; }
    }
}
