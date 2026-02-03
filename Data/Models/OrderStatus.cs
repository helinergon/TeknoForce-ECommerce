//OrderAtatus.cs

namespace TeknoForce.Data.Models
{
    public class OrderStatus
    {
        public int OrderStatusId { get; set; }

        public string Name { get; set; } = null!;

        public string ColorCode { get; set; } = null!;
        // Örn: #0d6efd, #198754

        public int SortOrder { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
