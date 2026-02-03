//ProductSpecification
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeknoForce.Data.Models
{
    public class ProductSpecification
    {
        [Key]
        public int ProductSpecificationId { get; set; }

        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }

        // TEKNİK ÖZELLİKLER
        public string? MaxNozzle { get; set; }
        public string? MotorPowerHP { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? WeightKg { get; set; }

        public string? SetContent { get; set; }
        public string? UsageAreas { get; set; }
        public string? UsableMaterials { get; set; }
    }
}
