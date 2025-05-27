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

        // Throttling settings
        private const int MaxFailedAttempts = 5;
        private static readonly TimeSpan LockoutDuration = TimeSpan.FromMinutes(1);
        // Key: IP, Value: (fail count, lockout until)
        private static readonly Dictionary<string, (int FailCount, DateTime? LockoutUntil)> _ipFailures = new();
        private static readonly object _lock = new();

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
            lock (_lock)
            {
                if (_ipFailures.TryGetValue(ip, out var entry))
                {
                    if (entry.LockoutUntil.HasValue && entry.LockoutUntil > now)
                    {
                        ErrorMessage = $"Too many failed login attempts. Please try again after {(entry.LockoutUntil.Value - now).Seconds} seconds.";
                        _logger.LogWarning("IP {IP} is locked out until {LockoutUntil}", ip, entry.LockoutUntil);
                        return Page();
                    }
                }
            }
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
                lock (_lock)
                {
                    if (_ipFailures.TryGetValue(ip, out var entry))
                    {
                        entry.FailCount++;
                        if (entry.FailCount >= MaxFailedAttempts)
                        {
                            entry.LockoutUntil = now.Add(LockoutDuration);
                        }
                        _ipFailures[ip] = entry;
                    }
                    else
                    {
                        _ipFailures[ip] = (1, null);
                    }
                }
                if (_ipFailures[ip].LockoutUntil.HasValue && _ipFailures[ip].LockoutUntil > now)
                {
                    ErrorMessage = $"Too many failed login attempts. Please try again after {( _ipFailures[ip].LockoutUntil.Value - now).Seconds} seconds.";
                }
                else
                {
                    ErrorMessage = "Invalid username or password";
                }
                _logger.LogWarning("Login failed for IP {IP}, count: {Count}, lockout: {Lockout}", ip, _ipFailures[ip].FailCount, _ipFailures[ip].LockoutUntil);
                return Page();
            }

            // On successful login, clear failure count
            lock (_lock)
            {
                if (_ipFailures.ContainsKey(ip))
                {
                    _ipFailures.Remove(ip);
                }
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