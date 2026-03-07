using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TeknoForce.Data;
using TeknoForce.Data.Models;

namespace TeknoForce.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public List<Category> Categories { get; set; } = new();

        public void OnGet()
        {
            Categories = _context.Categories
                .Include(c => c.Brand)
                .OrderBy(c => c.Brand!.Name)
                .ThenBy(c => c.Name)
                .ToList();
        }
    }
}
