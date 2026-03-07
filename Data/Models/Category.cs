namespace TeknoForce.Data.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }   
        public DateTime? UpdatedDate { get; set; }
        public int BrandId { get; set; }
        public Brand? Brand { get; set; } = null!;
        public ICollection<Product> Products { get; set; } = new List<Product>();

    }
}
