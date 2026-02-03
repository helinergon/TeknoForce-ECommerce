//OrderStatuses klasöründe create.cshtml.cs kodlarý:
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TeknoForce.Data;
using TeknoForce.Data.Models;

namespace TeknoForce.Pages.Admin.OrderStatuses
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;

        public CreateModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public OrderStatus OrderStatus { get; set; } = new();

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.OrderStatuses.Add(OrderStatus);
            _context.SaveChanges();

            return RedirectToPage("Index");
        }
    }
}

