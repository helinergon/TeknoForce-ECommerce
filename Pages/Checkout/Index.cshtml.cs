using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TeknoForce.Data;
using TeknoForce.Data.Models;
using System.Text.Json;

namespace TeknoForce.Pages.Checkout
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string AddressType { get; set; } = null!; // "Home" veya "Work"

        [BindProperty]
        public string? CompanyName { get; set; }
        [BindProperty]
        public string FullName { get; set; } = null!;

        [BindProperty]
        public string Phone { get; set; } = null!;

        [BindProperty]
        public string Email { get; set; } = null!;

        [BindProperty]
        public string City { get; set; } = null!;

        [BindProperty]
        public string District { get; set; } = null!;

        [BindProperty]
        public string Neighborhood { get; set; } = null!;

        [BindProperty]
        public string Street { get; set; } = null!;

        [BindProperty]
        public string BuildingNo { get; set; } = null!;

        [BindProperty]
        public string ApartmentNo { get; set; } = null!;

        [BindProperty]
        public string AddressNote { get; set; } = string.Empty;

        [BindProperty]
        public string PaymentMethod { get; set; } = "BankTransfer";

        public List<CartItem> Cart { get; set; } = new();

        public decimal Total => Cart.Sum(x => x.TotalPrice);

        public void OnGet()
        {
            Cart = GetCart();
        }

        public IActionResult OnPost()
        {
            Cart = GetCart();

            if (!Cart.Any())
                return RedirectToPage("/Cart/Index");

            if (AddressType == "Work" && string.IsNullOrWhiteSpace(CompanyName))
            {
                ModelState.AddModelError("", "İş adresi seçildiğinde firma adı zorunludur.");
                return Page();
            }


            var order = new Order
            {
                OrderNumber = Guid.NewGuid().ToString().Substring(0, 8).ToUpper(),
                UserId = 1,

                OrderStatusId = 5, // 5 = Ödeme Bekleniyor
                PaymentMethod = "BankTransfer",

                TotalAmount = Cart.Sum(x => x.TotalPrice),
                CreatedDate = DateTime.Now,

                FullName = FullName,
                Phone = Phone,
                Email = Email,
                AddressType = AddressType,
                CompanyName = AddressType == "Work" ? CompanyName : null,
                City = City,
                District = District,
                Neighborhood = Neighborhood,
                Street = Street,
                BuildingNo = BuildingNo,
                ApartmentNo = ApartmentNo,
                AddressNote = AddressNote
            };


            foreach (var item in Cart)
            {
                order.OrderItems.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                });
            }

            _context.Orders.Add(order);
            _context.SaveChanges();



            HttpContext.Session.Remove("Cart");


            return RedirectToPage("/Checkout/Success",
        new { orderNumber = order.OrderNumber });
        }

        private List<CartItem> GetCart()
        {
            var cartJson = HttpContext.Session.GetString("Cart");

            return string.IsNullOrEmpty(cartJson)
                ? new List<CartItem>()
                : JsonSerializer.Deserialize<List<CartItem>>(cartJson)!;
        }
    }
}