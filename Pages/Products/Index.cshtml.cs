//product klasöründeki index.cshtml.cs kodlarý:
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TeknoForce.Data;
using TeknoForce.Data.Models;


namespace TeknoForce.Pages.Admin.Products
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public List<Product> Products { get; set; } = new();
        [BindProperty(SupportsGet = true)]
        public int? BrandId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? CategoryId { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool? IsActive { get; set; }

        public SelectList BrandList { get; set; } = null!;
        public SelectList CategoryList { get; set; } = null!;

        public void OnGet()
        {
            IQueryable<Product> query = _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Specification)
                .Include(p => p.Images)
                .Include(p => p.Comments);

            if (BrandId.HasValue)
                query = query.Where(p => p.BrandId == BrandId.Value);

            if (CategoryId.HasValue)
                query = query.Where(p => p.CategoryId == CategoryId.Value);

            if (IsActive.HasValue)
                query = query.Where(p => p.IsActive == IsActive.Value);

            Products = query
                .OrderByDescending(p => p.CreatedDate)
                .ToList();

            LoadFilters();
        }
        public IActionResult OnPostToggleStatus(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == id);

            if (product == null)
                return NotFound();

            product.IsActive = !product.IsActive;
            _context.SaveChanges();

            return RedirectToPage();
        }
        public IActionResult OnPostDelete(int id)
        {
            var product = _context.Products
                .Include(p => p.Specification)
                .Include(p => p.Images)
                .FirstOrDefault(p => p.ProductId == id);

            if (product == null)
                return NotFound();

            _context.Products.Remove(product);
            _context.SaveChanges();

            return RedirectToPage();
        }
        private void LoadFilters()
        {
            BrandList = new SelectList(
                _context.Brands.Where(b => b.IsActive),
                "BrandId",
                "Name"
            );

            CategoryList = new SelectList(
                _context.Categories.Where(c => c.IsActive),
                "CategoryId",
                "Name"
            );
        }
    }

}
