using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TeknoForce.Data;
using TeknoForce.Data.Models;

namespace TeknoForce.Pages.Categories
{
    public class EditModel : PageModel
    {
        private readonly AppDbContext _context;

        public EditModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Category Category { get; set; } = null!;

        public SelectList BrandList { get; set; } = null!;

        // SAYFAYI AÇMA
        public IActionResult OnGet(int id)
        {
            Category = _context.Categories
                .Include(c => c.Brand)
                .FirstOrDefault(c => c.CategoryId == id)!;

            if (Category == null)
                return NotFound();

            LoadBrands();
            return Page();
        }

        // KAYDET
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                LoadBrands();
                return Page();
            }

            var categoryFromDb = _context.Categories
                .FirstOrDefault(c => c.CategoryId == Category.CategoryId);

            if (categoryFromDb == null)
                return NotFound();

            categoryFromDb.Name = Category.Name;
            categoryFromDb.BrandId = Category.BrandId;
            categoryFromDb.IsActive = Category.IsActive;
            categoryFromDb.UpdatedDate = DateTime.Now;

            _context.SaveChanges();

            return RedirectToPage("./Index");
        }

        // SÝL
        public IActionResult OnPostDelete(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.CategoryId == id);

            if (category == null)
                return NotFound();

            _context.Categories.Remove(category);
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
