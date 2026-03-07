using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TeknoForce.Data;
using TeknoForce.Data.Models;
using System.Text.Json;

namespace TeknoForce.Pages.Shop
{
    public class DetailsModel : PageModel
    {
        private readonly AppDbContext _context;

        public DetailsModel(AppDbContext context)
        {
            _context = context;
        }

        public Product Product { get; set; } = null!;

        [BindProperty]
        public Comment NewComment { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Specification)
                .Include(p => p.Medias)
                .Include(p => p.Images)
                .Include(p => p.Comments.Where(c => c.IsApproved))
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (Product == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
                return RedirectToPage(new { id });

            NewComment.ProductId = id;
            NewComment.IsApproved = false;
            NewComment.CreatedDate = DateTime.Now;

            _context.Comments.Add(NewComment);
            await _context.SaveChangesAsync();

            return RedirectToPage(new { id });
        }
        public IActionResult OnPostAddToCart(int productId)
        {
            var product = _context.Products
                .FirstOrDefault(p => p.ProductId == productId);

            if (product == null)
                return new JsonResult(new { success = false });

            var cartJson = HttpContext.Session.GetString("Cart");

            var cart = string.IsNullOrEmpty(cartJson)
                ? new List<CartItem>()
                : JsonSerializer.Deserialize<List<CartItem>>(cartJson)!;

            var existing = cart.FirstOrDefault(x => x.ProductId == productId);

            if (existing != null)
                existing.Quantity++;
            else
                cart.Add(new CartItem
                {
                    ProductId = product.ProductId,
                    ProductName = product.Name,
                    UnitPrice = product.PriceWithVat,
                    Quantity = 1
                });

            HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));

            return new JsonResult(new
            {
                success = true,
                count = cart.Sum(x => x.Quantity)
            });
        }

    }
}