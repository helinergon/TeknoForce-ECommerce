//OrderStatuses klasöründeki index.cshtml.cs kodlarý:
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TeknoForce.Data;
using TeknoForce.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace TeknoForce.Pages.Admin.OrderStatuses
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public List<OrderStatus> OrderStatuses { get; set; } = new();

        public void OnGet()
        {
            OrderStatuses = _context.OrderStatuses
                .OrderBy(x => x.SortOrder)
                .ToList();
        }
        public IActionResult OnPostToggleStatus(int id)
        {
            var status = _context.OrderStatuses
                .FirstOrDefault(x => x.OrderStatusId == id);

            if (status == null)
                return RedirectToPage();

            status.IsActive = !status.IsActive;
            _context.SaveChanges();

            return RedirectToPage();
        }
        public IActionResult OnPostDeactivate(int id)
        {
            var status = _context.OrderStatuses.FirstOrDefault(x => x.OrderStatusId == id);

            if (status == null)
                return NotFound();

            status.IsActive = false;
            _context.SaveChanges();

            return RedirectToPage();
        }


    }
}
