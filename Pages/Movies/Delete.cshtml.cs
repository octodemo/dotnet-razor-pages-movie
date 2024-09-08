using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPagesMovie.Data;
using RazorPagesMovie.Models;

namespace RazorPagesMovie.Pages.Movies
{
    public class DeleteModel : PageModel
    {
        private readonly RazorPagesMovie.Data.RazorPagesMovieContext _context;

        public DeleteModel(RazorPagesMovie.Data.RazorPagesMovieContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Movie Movie { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Invalid movie ID.";
                return RedirectToPage("./Index");
            }

            var movie = await _context.Movie.FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                TempData["ErrorMessage"] = "The movie you are trying to delete does not exist.";
                return RedirectToPage("./Index");
            }
            else
            {
                Movie = movie;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Invalid movie ID.";
                return RedirectToPage("./Index");
            }

            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                // Movie has already been deleted by another user
                TempData["ErrorMessage"] = "The movie you are trying to delete has already been deleted.";
                return RedirectToPage("./Index");
            }

            Movie = movie;
            _context.Movie.Remove(Movie);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle concurrency exception
                TempData["ErrorMessage"] = "The movie you are trying to delete has already been deleted by another user.";
                return RedirectToPage("./Index");
            }

            TempData["SuccessMessage"] = "Movie deleted successfully.";
            return RedirectToPage("./Index");
        }
    }
}