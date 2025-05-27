using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesMovie.Pages.Account;

namespace RazorPagesMovie.Pages
{
    public class TestLockoutResetModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string Ip { get; set; } = "127.0.0.1";

        public IActionResult OnGet()
        {
            return new JsonResult(new { success = true, ip = Ip });
        }
    }
}
