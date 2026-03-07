using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TeknoForce.Pages.Checkout
{
    public class SuccessModel : PageModel
    {
        public string OrderNumber { get; set; } = string.Empty;

        public void OnGet(string orderNumber)
        {
            OrderNumber = orderNumber;
        }
    }
}
