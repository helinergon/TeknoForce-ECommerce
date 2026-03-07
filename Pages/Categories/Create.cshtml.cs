using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TeknoForce.Data;
using TeknoForce.Data.Models;

namespace TeknoForce.Pages.Categories
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;

        public CreateModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Category Category { get; set; } = new();

        public SelectList BrandList { get; set; } = null!;

        public void OnGet()
        {
            LoadBrands();
        }

        public IActionResult OnPost()
        {
            Console.WriteLine("OnPost çalýþtý");

            if (!ModelState.IsValid)
            {
                LoadBrands(); 
                return Page();
            }

            Category.CreatedDate = DateTime.Now;
            Category.IsActive = true;

            _context.Categories.Add(Category);
            _context.SaveChanges();

            return RedirectToPage("./Index");
        }

        private void LoadBrands()
        {
            BrandList = new SelectList(
                _context.Brands.Where(b => b.IsActive),
                "BrandId",
                "Name"
            );
        }
    }
}
