using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPagesMovie.Data;
using RazorPagesMovie.Models;
using System.ComponentModel.DataAnnotations;

namespace RazorPagesMovie.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly RazorPagesMovieContext _context;
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(RazorPagesMovieContext context, ILogger<RegisterModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        [BindProperty]
        public RegisterInput RegisterUser { get; set; } = default!;

        public string? ErrorMessage { get; set; }

        public void OnGet()
        {
            ErrorMessage = null;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (await _context.Users.AnyAsync(u => u.Username == RegisterUser.Username))
            {
                ErrorMessage = "Username already exists";
                return Page();
            }

            var user = new User
            {
                Username = RegisterUser.Username,
                Password = RegisterUser.Password,
                Role = UserRole.Standard
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("UserRole", user.Role.ToString());

            return RedirectToPage("/Index");
        }
    }

    public class RegisterInput
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        [Display(Name = "Username")]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Role")]
        public UserRole Role { get; set; } = UserRole.Standard;
    }
}