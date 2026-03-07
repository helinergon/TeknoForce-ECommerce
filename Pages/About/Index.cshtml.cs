using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TeknoForce.Data;
using TeknoForce.Data.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace TeknoForce.Pages.Admin.AboutContents
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public AboutContent About { get; set; } = new();


        public void OnGet()
        {
            // DB'deki ilk (ve tek) About kaydýný al
            var aboutFromDb = _context.AboutContents.FirstOrDefault();

            if (aboutFromDb != null)
            {
                About = aboutFromDb;
            }
            else
            {
                // Ýlk kez oluþturulacak
                About = new AboutContent
                {
                    IsActive = true
                };
            }
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var aboutFromDb = _context.AboutContents.FirstOrDefault();

            if (aboutFromDb == null)
            {
                // Ýlk kayýt
                About.UpdatedDate = DateTime.Now;
                _context.AboutContents.Add(About);
            }
            else
            {
                // Güncelleme
                aboutFromDb.Title = About.Title;
                aboutFromDb.Content = About.Content;
                aboutFromDb.IsActive = About.IsActive;
                aboutFromDb.UpdatedDate = DateTime.Now;
            }

            _context.SaveChanges();

            return RedirectToPage();
        }

    }
}
