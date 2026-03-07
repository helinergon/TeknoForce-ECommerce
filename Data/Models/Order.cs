//order.cs

using System.ComponentModel.DataAnnotations.Schema;

namespace TeknoForce.Data.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        public string OrderNumber { get; set; } = null!;


        public string FullName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;

        public string AddressType { get; set; } = null!; // Home / Work
        public string? CompanyName { get; set; }

        public string City { get; set; } = null!;
        public string District { get; set; } = null!;
        public string Neighborhood { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string BuildingNo { get; set; } = null!;
        public string? ApartmentNo { get; set; }
        public string? AddressNote { get; set; }


        public int UserId { get; set; }

        public int OrderStatusId { get; set; }
        public OrderStatus OrderStatus { get; set; } = null!;

        // KDV DAHİL TOPLAM
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public string PaymentMethod { get; set; } = null!;



    }
}
