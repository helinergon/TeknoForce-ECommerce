using TeknoForce.Data;

namespace TeknoForce.Data.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = null!;

        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        // Hesaplanan alan (DB'ye yazılmaz)
        public decimal TotalPrice => UnitPrice * Quantity;
    }
}
