using Microsoft.AspNetCore.Mvc.RazorPages;
using TeknoForce.Data;
using TeknoForce.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace TeknoForce.Pages.Admin.Contact.Branches
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public List<ContactBranch> Branches { get; set; } = new();

        public void OnGet()
        {
            Branches = _context.ContactBranches
                .OrderBy(b => b.CreatedDate)
                .ToList();
        }
    }
}
