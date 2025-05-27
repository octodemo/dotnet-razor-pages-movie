using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPagesMovie.Pages.Account
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            HttpContext.Session.Clear();
            // Expire the session cookie
            if (Request.Cookies[".AspNetCore.Session"] != null)
            {
                Response.Cookies.Append(".AspNetCore.Session", "", new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(-1),
                    HttpOnly = true,
                    Secure = true,
                    IsEssential = true
                });
            }
            return RedirectToPage("/Index");
        }
    }
}
