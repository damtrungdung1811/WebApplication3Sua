using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication3.Models
{
    public class Checklist
    {
        [Key]
        public int ChecklistID { get; set; }

        [Required, MaxLength(200)]
        public string Ten { get; set; }

        [MaxLength(500)]
        public string MoTa { get; set; }

        public ICollection<ChecklistItem> Items { get; set; }
    }
}