//OrderStatuses klasöründeki edit.cshtml.cs kodlarý:
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TeknoForce.Data;
using TeknoForce.Data.Models;

namespace TeknoForce.Pages.Admin.OrderStatuses
{
    public class EditModel : PageModel
    {
        private readonly AppDbContext _context;

        public EditModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public OrderStatus OrderStatus { get; set; } = new();

        public IActionResult OnGet(int id)
        {
            OrderStatus = _context.OrderStatuses
                .FirstOrDefault(x => x.OrderStatusId == id);

            if (OrderStatus == null)
                return NotFound();

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.OrderStatuses.Update(OrderStatus);
            _context.SaveChanges();

            return RedirectToPage("Index");
        }
    }
}
