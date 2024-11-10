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
                return Page();
            }

            var user = await _context.Users
                .Include(u => u.Movies) // Include movies related to the user
                .FirstOrDefaultAsync(u => u.Username == LoginInput.Username);

            if (user == null || user.Password != LoginInput.Password)
            {
                ErrorMessage = "Invalid username or password";
                return Page();
            }

            // Set session variables
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("Username", user.Username);

            _logger.LogInformation("User {Username} logged in at {Time} with {MovieCount} movies", 
                user.Username, DateTime.UtcNow, user.Movies.Count);

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