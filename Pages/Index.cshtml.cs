using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeknoForce.Data.Models;
using TeknoForce.Data;   // kendi namespace’ine göre düzelt

namespace TeknoForce.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }
        public List<Product> BestSellers { get; set; } = new();
        public List<Brand> HomeBrands { get; set; }

        public List<Product> NewProducts { get; set; } = new();
        public List<Category> HomeCategories { get; set; } = new();
        public Dictionary<int, int> CategoryProductCounts { get; set; } = new();
        public List<ContactBranch> Branches { get; set; }

   

        public void OnGet()
        {
            HomeBrands = _context.Brands
                .Where(x => x.IsActive)
                .OrderBy(x => x.BrandId)
                .Take(2)
                .ToList();

            NewProducts = _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Medias)
                .Where(p => p.IsActive)
                .OrderByDescending(p => p.CreatedDate)
                .Take(4)
                .ToList();

            CategoryProductCounts = _context.Products
                .Where(p => p.IsActive)
                .GroupBy(p => p.CategoryId)
                .ToDictionary(g => g.Key, g => g.Count());

            var bestSellerData = _context.OrderItems
                .GroupBy(oi => oi.ProductId)
                .Select(g => new
            {
            ProductId = g.Key,
            TotalSold = g.Sum(x => x.Quantity)
            })
                .OrderByDescending(x => x.TotalSold)
                .Take(4)
                .ToList();

            if (bestSellerData.Any())
            {
                BestSellers = bestSellerData
                    .Join(_context.Products
                        .Include(p => p.Brand)
                        .Include(p => p.Medias),
                        grouped => grouped.ProductId,
                        product => product.ProductId,
                        (grouped, product) => product)
                    .Where(p => p.IsActive)
                    .ToList();
            }
            else
            {
                var firstTwo = _context.Products
                    .Include(p => p.Brand)
                    .Include(p => p.Medias)
                    .Where(p => p.IsActive)
                    .OrderBy(p => p.CreatedDate)      // ilk eklenen
                    .Take(2)
                    .ToList();

                var lastTwo = _context.Products
                    .Include(p => p.Brand)
                    .Include(p => p.Medias)
                    .Where(p => p.IsActive)
                    .OrderByDescending(p => p.CreatedDate)  // son eklenen
                    .Take(2)
                    .ToList();

                BestSellers = firstTwo
                    .Concat(lastTwo)
                    .Distinct()
                    .Take(4)
                    .ToList();
            }

            Branches = _context.ContactBranches
              .Include(b => b.ContactPhones)
              .Where(b => b.IsActive)
              .ToList();


            HomeCategories = _context.Categories
                .Include(c => c.Products)
                .Where(c => c.IsActive)
                .OrderBy(c => c.CategoryId)
                .Take(4)
                .ToList();
        }

    }


}
