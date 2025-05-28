using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RazorPagesMovie.Models;

namespace RazorPagesMovie.Pages.Movies
{
    public class IndexModel : PageModel
    {
        private readonly RazorPagesMovie.Data.RazorPagesMovieContext _context;

        public IndexModel(RazorPagesMovie.Data.RazorPagesMovieContext context)
        {
            _context = context;
        }

        public IList<Movie> Movie { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        public SelectList? Genres { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? MovieGenre { get; set; }

        public string? UserRole { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var sessionEnabled = Environment.GetEnvironmentVariable("DISABLE_SESSION")?.ToLower() != "true";
            // Only check session if we have a valid HttpContext with Session functionality
            if (sessionEnabled && HttpContext?.Session != null && HttpContext.Session.GetInt32("UserId") == null)
            {
                if (Response != null)
                {
                    Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate, max-age=0";
                    Response.Headers["Pragma"] = "no-cache";
                    Response.Headers["Expires"] = "0";
                }
                return RedirectToPage("/Account/Login");
            }
            // Get user role from session (safely)
            UserRole = sessionEnabled ? HttpContext?.Session?.GetString("UserRole") : null;
            // Add anti-cache headers to prevent browser from caching this page
            if (Response != null)
            {
                Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate, max-age=0";
                Response.Headers["Pragma"] = "no-cache";
                Response.Headers["Expires"] = "0";
            }
            // Use LINQ to get list of genres.
            IQueryable<string> genreQuery = from m in _context.Movie
                                            orderby m.Genre
                                            select m.Genre;
            var movies = from m in _context.Movie
                         select m;
            if (!string.IsNullOrEmpty(SearchString))
            {
                movies = movies.Where(s => s.Title.Contains(SearchString));
            }
            if (!string.IsNullOrEmpty(MovieGenre))
            {
                movies = movies.Where(x => x.Genre == MovieGenre);
            }
            Genres = new SelectList(await genreQuery.Distinct().ToListAsync());
            Movie = await movies.ToListAsync();
            return Page();
        }
    }
}
