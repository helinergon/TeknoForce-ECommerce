using System;
using System.ComponentModel.DataAnnotations;

namespace TeknoForce.Data.Models
{
    public class ContactContent
    {
        [Key]
        public int ContactId { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }
        public string MapEmbed { get; set; }
        public bool IsActive { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
