using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPagesMovie.Data;
using RazorPagesMovie.Models;

namespace RazorPagesMovie.Pages.Movies
{
    public class AddToFavoritesModel : PageModel
    {
        private readonly RazorPagesMovieContext _context;

        public AddToFavoritesModel(RazorPagesMovieContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Movie? Movie { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Movie = await _context.Movie.FirstOrDefaultAsync(m => m.Id == id);

            if (Movie == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Movies)
                .FirstOrDefaultAsync(u => u.Username == User.Identity.Name);

            if (user == null)
            {
                return NotFound();
            }

            user.Movies.Add(Movie);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Movie = await _context.Movie.FirstOrDefaultAsync(m => m.Id == id);

            if (Movie == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Movies)
                .FirstOrDefaultAsync(u => u.Username == User.Identity.Name);

            if (user == null)
            {
                return NotFound();
            }

            user.Movies.Add(Movie);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}