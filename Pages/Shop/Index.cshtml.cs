using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TeknoForce.Data;
using TeknoForce.Data.Models;

namespace TeknoForce.Pages.Shop
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public List<Product> Products { get; set; } = new();
        public List<Category> Categories { get; set; }
        public List<Brand> Brands { get; set; }
        public int? SelectedCategoryId { get; set; }
        public int? SelectedBrandId { get; set; }

        public async Task OnGetAsync(int? categoryId, int? brandId)
        {
            SelectedCategoryId = categoryId;
            SelectedBrandId = brandId;
            Categories = await _context.Categories.ToListAsync();

            Brands = await _context.Brands.ToListAsync();

            var query = _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Medias)
                .Where(p => p.IsActive)
                .AsQueryable();

            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            if (brandId.HasValue)
            {
                query = query.Where(p => p.BrandId == brandId.Value);
            }

            Products = await query.ToListAsync();

        }
    }
}