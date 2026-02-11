using System;
using System.ComponentModel.DataAnnotations;

namespace TeknoForce.Data.Models
{
    public class ContactBranch
    {
        [Key]
        public int ContactBranchId { get; set; }

        [Required]
        public string ContactName { get; set; }        // Şube Adı
        public string Address { get; set; }     // Açık adres
        public string? MapIframe { get; set; }  // Google Maps iframe
        public bool IsActive { get; set; }
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public ICollection<ContactPhone> ContactPhones { get; set; } = new List<ContactPhone>();

    }
}