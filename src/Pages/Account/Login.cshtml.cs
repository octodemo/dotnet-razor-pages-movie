using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPagesMovie.Data;
using RazorPagesMovie.Models;
using System.ComponentModel.DataAnnotations;

namespace RazorPagesMovie.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly RazorPagesMovieContext _context;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(RazorPagesMovieContext context, ILogger<LoginModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        [BindProperty]
        public LoginInput LoginInput { get; set; } = default!;

        public string? ErrorMessage { get; set; }

        public void OnGet()
        {
            // Clear any existing error message
            ErrorMessage = null;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ErrorMessage = "Invalid input. Please check your username and password.";
                _logger.LogWarning("Invalid input: {ModelStateErrors}", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return Page();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == LoginInput.Username);

            if (user == null)
            {
                ErrorMessage = "Invalid username or password";
                _logger.LogWarning("Login failed: Invalid username '{Username}'", LoginInput.Username);
                return Page();
            }

            if (user.Password != LoginInput.Password)
            {
                ErrorMessage = "Invalid username or password";
                _logger.LogWarning("Login failed: Invalid password for username '{Username}'", LoginInput.Username);
                return Page();
            }

            // Set session variables
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("Username", user.Username);

            // Retrieve user role
            var role = user.Role.ToString();

            // Log user role to console
            Console.WriteLine($"User {user.Username} logged in at {DateTime.UtcNow} with role: {role}");

            // Log user role using logger
            _logger.LogInformation("User {Username} logged in at {Time} with role: {Role}", 
                user.Username, DateTime.UtcNow, role);

            // Store role in Session
            HttpContext.Session.SetString("UserRole", role);

            return RedirectToPage("/Movies/Index");
        }
    }

    public class LoginInput
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;
    }
}