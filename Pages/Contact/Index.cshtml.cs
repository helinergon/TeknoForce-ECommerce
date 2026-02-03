using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TeknoForce.Data;
using TeknoForce.Data.Models;
using System.Linq;

namespace TeknoForce.Pages.Admin.Contact
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public ContactContent Contact { get; set; } = new();
        public void OnGet()
        {
            // DB'deki ilk (ve tek) Contact kaydýný al
            var contactFromDb = _context.ContactContents.FirstOrDefault();

            if (contactFromDb != null)
            {
                Contact = contactFromDb;
            }
            else
            {
                Contact = new ContactContent
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

            var contactFromDb = _context.ContactContents.FirstOrDefault();

            if (contactFromDb == null)
            {
                // Ýlk kayýt
                Contact.UpdatedDate = DateTime.Now;
                _context.ContactContents.Add(Contact);
            }
            else
            {
                // Güncelleme
                contactFromDb.Address = Contact.Address;
                contactFromDb.Phone = Contact.Phone;
                contactFromDb.Email = Contact.Email;
                contactFromDb.MapEmbed = Contact.MapEmbed;
                contactFromDb.IsActive = Contact.IsActive;
                contactFromDb.UpdatedDate = DateTime.Now;
            }

            _context.SaveChanges();

            return RedirectToPage();
        }
    }
}
