using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TeknoForce.Data;
using TeknoForce.Data.Models;

namespace TeknoForce.Pages.Admin.Contact.Branches
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;

        public CreateModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ContactBranch Branch { get; set; } = new();

        public void OnGet()
        {
            // Varsayýlanlar
            Branch.IsActive = true;
        }

        public IActionResult OnPost()
        {
            Console.WriteLine("ONPOST ÇALIÞTI");

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                throw new Exception(string.Join(" | ", errors));
            }

            _context.ContactBranches.Add(Branch);
            _context.SaveChanges();

            return RedirectToPage("Index");
        }


    }
}
