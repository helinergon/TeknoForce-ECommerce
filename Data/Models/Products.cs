//Products.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeknoForce.Data.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        // İLİŞKİLER
        public int BrandId { get; set; }
        public int CategoryId { get; set; }

        [ForeignKey(nameof(BrandId))]
        public Brand? Brand { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category? Category { get; set; }

        // GENEL BİLGİLER
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = null!;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public int Stock { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // NAVIGATION
        public ProductSpecification? Specification { get; set; }
        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
    }
}