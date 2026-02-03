using System;
using System.ComponentModel.DataAnnotations;

namespace TeknoForce.Data.Models
{
    public class ContactPhone
    {
        [Key]
        public int ContactPhoneId { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public string label { get; set; }
        public bool IsActive { get; set; }
        public int SortOrder { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
