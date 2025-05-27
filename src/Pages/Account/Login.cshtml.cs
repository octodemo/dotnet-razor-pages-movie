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
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            DateTime now = DateTime.UtcNow;
            // LOCKOUT DISABLED: No lockout or throttling in demo mode
            // if (!ModelState.IsValid)
            if (!ModelState.IsValid)
            {
                ErrorMessage = "Invalid input. Please check your username and password.";
                _logger.LogWarning("Invalid input: {ModelStateErrors}", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return Page();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == LoginInput.Username);

            if (user == null || user.Password != LoginInput.Password)
            {
                // LOCKOUT DISABLED: Always allow login attempts, no fail count or lockout
                ErrorMessage = "Invalid username or password";
                _logger.LogWarning("Login failed for IP {IP}", ip);
                return Page();
            }

            // On successful login, clear failure count (no-op)
            // set session variables
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("Username", user.Username);

            // retrieve user role
            var role = user.Role.ToString();

            // log user role to console
            Console.WriteLine($"User {user.Username} logged in at {DateTime.UtcNow} with role: {role}");

            // log user role using logger
            _logger.LogInformation("User {Username} logged in at {Time} with role: {Role}", 
                user.Username, DateTime.UtcNow, role);

            // store role in Session
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