using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TeknoForce.Data;
using TeknoForce.Data.Models;

namespace TeknoForce.Pages.Brands
{
    public class EditModel : PageModel
    {
        private readonly AppDbContext _context;

        public EditModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Brand? Brand { get; set; } = default!;

        [BindProperty]
        public IFormFile? LogoFile { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Brand = await _context.Brands.FirstOrDefaultAsync(x => x.BrandId == id);

            if (Brand == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var brandFromDb = await _context.Brands.FirstOrDefaultAsync(x => x.BrandId == Brand.BrandId);

            if (brandFromDb == null)
                return NotFound();

            brandFromDb.Description = Brand.Description;
            brandFromDb.IsActive = Brand.IsActive;

            if (LogoFile != null)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/brands");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid() + Path.GetExtension(LogoFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await LogoFile.CopyToAsync(stream);

                brandFromDb.LogoPath = "/uploads/brands/" + fileName;
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
