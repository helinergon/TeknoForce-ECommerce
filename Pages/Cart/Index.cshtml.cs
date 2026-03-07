using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TeknoForce.Data.Models;
using System.Text.Json;

namespace TeknoForce.Pages.Cart
{
    public class IndexModel : PageModel
    {
        public List<CartItem> CartItems { get; set; } = new();

        public void OnGet()
        {
            var cartJson = HttpContext.Session.GetString("Cart");

            if (!string.IsNullOrEmpty(cartJson))
            {
                CartItems = JsonSerializer.Deserialize<List<CartItem>>(cartJson)!;
            }
        }

        public decimal Total => CartItems.Sum(x => x.TotalPrice);

        public IActionResult OnPostIncrease(int productId)
        {
            var cart = GetCart();

            var item = cart.FirstOrDefault(x => x.ProductId == productId);

            if (item != null)
                item.Quantity++;

            SaveCart(cart);

            return new JsonResult(new
            {
                success = true,
                total = cart.Sum(x => x.TotalPrice),
                count = cart.Sum(x => x.Quantity)
            });
        }

        public IActionResult OnPostDecrease(int productId)
        {
            var cart = GetCart();

            var item = cart.FirstOrDefault(x => x.ProductId == productId);

            if (item != null)
            {
                item.Quantity--;

                if (item.Quantity <= 0)
                    cart.Remove(item);
            }

            SaveCart(cart);

            return new JsonResult(new
            {
                success = true,
                total = cart.Sum(x => x.TotalPrice),
                count = cart.Sum(x => x.Quantity)
            });
        }

        public IActionResult OnPostRemove(int productId)
        {
            var cart = GetCart();

            var item = cart.FirstOrDefault(x => x.ProductId == productId);

            if (item != null)
                cart.Remove(item);

            SaveCart(cart);

            return new JsonResult(new
            {
                success = true,
                total = cart.Sum(x => x.TotalPrice),
                count = cart.Sum(x => x.Quantity)
            });
        }

        private List<CartItem> GetCart()
        {
            var cartJson = HttpContext.Session.GetString("Cart");

            return string.IsNullOrEmpty(cartJson)
                ? new List<CartItem>()
                : JsonSerializer.Deserialize<List<CartItem>>(cartJson)!;
        }

        private void SaveCart(List<CartItem> cart)
        {
            HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));
        }
    }
}