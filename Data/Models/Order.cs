//order.cs

namespace TeknoForce.Data.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        public string OrderNumber { get; set; } = null!;
        // Örn: TF-2026-000123

        public int UserId { get; set; }
        // İleride AspNetUsers FK bağlanacak

        public int OrderStatusId { get; set; }
        public OrderStatus OrderStatus { get; set; } = null!;

        public decimal TotalAmount { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
