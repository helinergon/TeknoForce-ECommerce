//orders klasöründeki details.cshtml.cs kodları
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TeknoForce.Data;
using TeknoForce.Data.Models;

namespace TeknoForce.Pages.Admin.Orders
{
    public class DetailsModel : PageModel
    {
        public List<SelectListItem> StatusList { get; set; } = new();

        private readonly AppDbContext _context;

        public DetailsModel(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult OnPostUpdateStatus(int orderId, int orderStatusId)
        {
            var order = _context.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.OrderStatus)
                .FirstOrDefault(x => x.OrderId == orderId);

            if (order == null)
                return NotFound();

            var newStatus = _context.OrderStatuses
                .FirstOrDefault(x => x.OrderStatusId == orderStatusId);

            if (newStatus == null)
                return BadRequest();

            var currentStatusName = order.OrderStatus.Name;
            var newStatusName = newStatus.Name;

            // Teslim edilen sipariş kilit
            if (currentStatusName == "Teslim Edildi")
                return BadRequest("Teslim edilen sipariş değiştirilemez");

            // ❌ İptalden başka bir duruma geçişte STOĞA DOKUNMA
            if (currentStatusName == "İptal Edildi" && newStatusName != "İptal Edildi")
            {
                // HİÇBİR ŞEY YAPMA
            }

            // ✅ İlk kez iptal ediliyorsa → stok geri ekle
            if (newStatusName == "İptal Edildi" && currentStatusName != "İptal Edildi")
            {
                foreach (var item in order.OrderItems)
                {
                    var product = _context.Products
                        .FirstOrDefault(p => p.ProductId == item.ProductId);

                    if (product != null)
                        product.Stock += item.Quantity;
                }
            }

            order.OrderStatusId = orderStatusId;
            _context.SaveChanges();

            return RedirectToPage(new { id = orderId });
        }



        public Order Order { get; set; } = null!;
        public List<OrderItem> OrderItems { get; set; } = new();

        public IActionResult OnGet(int id)
        {
            Order = _context.Orders
                .Include(o => o.OrderStatus)
                .FirstOrDefault(o => o.OrderId == id);
            StatusList = _context.OrderStatuses
                .Where(x => x.IsActive)
                .OrderBy(x => x.SortOrder)
                .Select(x => new SelectListItem
                 {
                Value = x.OrderStatusId.ToString(),
                Text = x.Name
                 })
                .ToList();


            if (Order == null)
                return NotFound();

            OrderItems = _context.OrderItems
                .Include(oi => oi.Product)
                .Where(oi => oi.OrderId == id)
                .ToList();

            return Page();
        }
    }
}