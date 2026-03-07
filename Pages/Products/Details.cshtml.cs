using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TeknoForce.Data;
using TeknoForce.Data.Models;

namespace TeknoForce.Pages.Admin.Products

{
    public class DetailsModel : PageModel
    {
        private readonly AppDbContext _context;

        public DetailsModel(AppDbContext context)
        {
            _context = context;
        }

        public Product? Product { get; set; }

        public List<Comment> ApprovedComments { get; set; } = new();

        [BindProperty]
        public Comment NewComment { get; set; } = new();

        public IActionResult OnGet(int id)
        {
            Product = _context.Products
                .Include(p => p.Specification)
                .Include(p => p.Images)
                .FirstOrDefault(p => p.ProductId == id && p.IsActive);

            if (Product == null)
                return NotFound();

            ApprovedComments = _context.Comments
                .Where(c => c.ProductId == id && c.IsApproved)
                .OrderByDescending(c => c.CreatedDate)
                .ToList();

            return Page();
        }

        public IActionResult OnPost(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
                return NotFound();

            if (!ModelState.IsValid)
                return RedirectToPage(new { id });

            NewComment.ProductId = id;
            NewComment.IsApproved = false;
            NewComment.CreatedDate = DateTime.Now;

            _context.Comments.Add(NewComment);
            _context.SaveChanges();

            return RedirectToPage(new { id });
        }
    }
}
