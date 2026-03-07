using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TeknoForce.Pages
{
    public class AdminBasePageModel : PageModel
    {
        public override void OnPageHandlerExecuting(
            Microsoft.AspNetCore.Mvc.Filters.PageHandlerExecutingContext context)
        {
            var path = context.HttpContext.Request.Path.Value;

            // LOGIN SAYFASI KONTROL DIŞI
            if (path.StartsWith("/Login"))
            {
                base.OnPageHandlerExecuting(context);
                return;
            }

            // SADECE ADMIN KLASÖRLERİ
            if (path.StartsWith("/Dashboard") ||
                path.StartsWith("/Products") ||
                path.StartsWith("/Categories") ||
                path.StartsWith("/Brands") ||
                path.StartsWith("/Orders") ||
                path.StartsWith("/Settings"))
            {
                var adminId = context.HttpContext.Session.GetInt32("AdminUserId");

                if (adminId == null)
                {
                    context.Result = new RedirectToPageResult("/Login");
                    return;
                }
            }

            base.OnPageHandlerExecuting(context);
        }
    }
}