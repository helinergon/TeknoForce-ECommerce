using System.ComponentModel.DataAnnotations;
namespace TeknoForce.Data.Models
{
    public class AboutContent
    {
        [Key]
        public int AboutId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = null!;

        [Required]
        public string Content { get; set; } = null!;

        public bool IsActive { get; set; } = true;

        public DateTime UpdatedDate { get; set; } = DateTime.Now;
    }
}
