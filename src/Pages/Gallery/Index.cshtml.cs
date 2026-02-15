using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPagesMovie.Pages.Gallery
{
    /// <summary>
    /// Page model for the Art Gallery display page
    /// </summary>
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _galleryLogger;

        public IndexModel(ILogger<IndexModel> galleryLogger)
        {
            _galleryLogger = galleryLogger;
        }

        /// <summary>
        /// Handle GET requests for the gallery page
        /// </summary>
        public void OnGet()
        {
            var accessTime = DateTime.UtcNow;
            _galleryLogger.LogInformation("Art Gallery accessed at {AccessTime}", accessTime);
        }
    }
}
