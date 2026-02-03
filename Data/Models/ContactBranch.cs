using System;
using System.ComponentModel.DataAnnotations;

namespace TeknoForce.Data.Models
{
    public class ContactBranch
    {
        [Key]
        public int ContactBranchId { get; set; }
        [Required]
        public string Title { get; set; } 
        [Required]
        public string Address { get; set; }
        public string MapEmbed { get; set; }
        public bool IsActive { get; set; }
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
        public object CreatedDate { get; internal set; }
    }
}
