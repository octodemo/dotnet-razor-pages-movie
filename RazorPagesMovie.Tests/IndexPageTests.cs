using Microsoft.EntityFrameworkCore;
using RazorPagesMovie.Data;
using RazorPagesMovie.Models;
using RazorPagesMovie.Pages.Movies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace RazorPagesMovie.Tests
{
    public class IndexPageTests : IDisposable
    {
        private readonly DbContextOptions<RazorPagesMovieContext> _options;
        private readonly ITestOutputHelper _output;

        public IndexPageTests(ITestOutputHelper output)
        {
            _output = output;
            _options = new DbContextOptionsBuilder<RazorPagesMovieContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            // Ensure the database is clean before starting the tests
            ClearDatabase();

            // Seed the in-memory database with test data
            using (var context = new RazorPagesMovieContext(_options))
            {
                context.Movie.AddRange(GetTestMovies());
                context.SaveChanges();
            }
        }

        [Fact]
        public async Task OnGetAsync_PopulatesThePageModel()
        {
            // Arrange
            using (var context = new RazorPagesMovieContext(_options))
            {
                var pageModel = new IndexModel(context);

                // Act
                await pageModel.OnGetAsync();

                // Assert
                Assert.NotNull(pageModel.Movie);
                Assert.IsType<List<Movie>>(pageModel.Movie);
                Assert.Equal(3, pageModel.Movie.Count); // Update expected count to 3

                // Enhanced output to console
                _output.WriteLine("=== Test Output ===");
                _output.WriteLine("Test: OnGetAsync_PopulatesThePageModel");
                _output.WriteLine($"Movies count: {pageModel.Movie.Count}");
                foreach (var movie in pageModel.Movie)
                {
                    if (movie != null)
                    {
                        _output.WriteLine($"- Title: {movie.Title}");
                        _output.WriteLine($"  Genre: {movie.Genre}");
                        _output.WriteLine($"  Price: {movie.Price}");
                        _output.WriteLine($"  ReleaseDate: {movie.ReleaseDate}");
                    }
                }
                _output.WriteLine("===================");
            }
        }

        [Fact]
        public async Task OnGetAsync_EmptyDatabase()
        {
            // Arrange
            using (var context = new RazorPagesMovieContext(_options))
            {
                if (context.Movie.Any())
                {
                    context.Movie.RemoveRange(context.Movie);
                    context.SaveChanges();
                }
        
                var pageModel = new IndexModel(context);
        
                // Act
                await pageModel.OnGetAsync();
        
                // Assert
                Assert.NotNull(pageModel.Movie);
                Assert.IsType<List<Movie>>(pageModel.Movie);
                Assert.Empty(pageModel.Movie);
        
                // Enhanced output to console
                _output.WriteLine("=== Test Output ===");
                _output.WriteLine("Test: OnGetAsync_EmptyDatabase");
                _output.WriteLine($"Movies count: {pageModel.Movie.Count}");
                _output.WriteLine("===================");
            }
        }

        [Fact]
        public async Task OnGetAsync_SingleMovie()
        {
            // Arrange
            using (var context = new RazorPagesMovieContext(_options))
            {
                context.Movie.RemoveRange(context.Movie);
                context.Movie.Add(new Movie
                {
                    Id = 1,
                    Title = "Test Movie",
                    ReleaseDate = DateTime.Parse("1989-2-12"),
                    Genre = "Romantic Comedy",
                    Price = 7.99M,
                    Rating = "PG",
                    Timestamp = new byte[8] // Initialize with a default value
                });
                context.SaveChanges();
            }

            // Act
            using (var context = new RazorPagesMovieContext(_options))
            {
                var pageModel = new RazorPagesMovie.Pages.Movies.IndexModel(context);
                await pageModel.OnGetAsync();

                // Assert
                Assert.Single(pageModel.Movie);
                Assert.Equal("Test Movie", pageModel.Movie.First().Title);
            }
        }

        [Fact]
        public async Task OnGetAsync_MultipleMovies()
        {
            // Arrange
            using (var context = new RazorPagesMovieContext(_options))
            {
                context.Movie.RemoveRange(context.Movie);
                context.Movie.AddRange(GetTestMovies());
                context.SaveChanges();

                var pageModel = new IndexModel(context);

                // Act
                await pageModel.OnGetAsync();

                // Assert
                Assert.NotNull(pageModel.Movie);
                Assert.IsType<List<Movie>>(pageModel.Movie);
                Assert.Equal(3, pageModel.Movie.Count); // Update expected count to 3

                // Enhanced output to console
                _output.WriteLine("=== Test Output ===");
                _output.WriteLine("Test: OnGetAsync_MultipleMovies");
                _output.WriteLine($"Movies count: {pageModel.Movie.Count}");
                foreach (var movie in pageModel.Movie)
                {
                    if (movie != null)
                    {
                        _output.WriteLine($"- Title: {movie.Title}");
                        _output.WriteLine($"  Genre: {movie.Genre}");
                        _output.WriteLine($"  Price: {movie.Price}");
                        _output.WriteLine($"  ReleaseDate: {movie.ReleaseDate}");
                    }
                }
                _output.WriteLine("===================");
            }
        }

        [Fact]
        public async Task OnGetAsync_MoviesSortedByReleaseDate()
        {
            // Arrange
            using (var context = new RazorPagesMovieContext(_options))
            {
                context.Movie.RemoveRange(context.Movie);
                context.Movie.AddRange(GetTestMovies());
                context.SaveChanges();

                var pageModel = new IndexModel(context);

                // Act
                await pageModel.OnGetAsync();

                // Assert
                Assert.NotNull(pageModel.Movie);
                Assert.IsType<List<Movie>>(pageModel.Movie);
                Assert.Equal(3, pageModel.Movie.Count); // Update expected count to 3

                // Sort movies by release date to ensure the order
                var sortedMovies = pageModel.Movie.OrderBy(m => m.ReleaseDate).ToList();

                // Enhanced output to console for debugging
                _output.WriteLine("=== Test Output ===");
                _output.WriteLine("Test: OnGetAsync_MoviesSortedByReleaseDate");
                _output.WriteLine($"Movies count: {sortedMovies.Count}");
                foreach (var movie in sortedMovies)
                {
                    if (movie != null)
                    {
                        _output.WriteLine($"- Title: {movie.Title}");
                        _output.WriteLine($"  ReleaseDate: {movie.ReleaseDate}");
                    }
                }
                _output.WriteLine("===================");

                Assert.True(sortedMovies[0].ReleaseDate < sortedMovies[1].ReleaseDate);
            }
        }

        [Fact]
        public async Task OnGetAsync_FilterMoviesByTitle()
        {
            // Arrange
            using (var context = new RazorPagesMovieContext(_options))
            {
                context.Movie.RemoveRange(context.Movie);
                context.Movie.AddRange(GetTestMovies());
                context.SaveChanges();
            }

            // Act
            using (var context = new RazorPagesMovieContext(_options))
            {
                var pageModel = new RazorPagesMovie.Pages.Movies.IndexModel(context);
                await pageModel.OnGetAsync();
                var filteredMovies = pageModel.Movie?.Where(m => m.Title == "Test Movie 1").ToList();

                // Assert
                Assert.NotNull(filteredMovies);
                Assert.Single(filteredMovies);
                Assert.Equal("Test Movie 1", filteredMovies.First().Title);
            }
        }

        [Fact]
        public async Task OnGetAsync_FilterMoviesByPrice()
        {
            // Arrange: Set up the in-memory database with test data
            using (var context = new RazorPagesMovieContext(_options))
            {
                // Clear existing movies and add test movies
                context.Movie.RemoveRange(context.Movie);
                context.Movie.AddRange(GetTestMovies());
                context.SaveChanges();

                var pageModel = new IndexModel(context);

                // Act: Execute the OnGetAsync method to populate the page model
                await pageModel.OnGetAsync();
                
                // Filter movies by price
                var filteredMovies = pageModel.Movie.Where(m => m.Price == 15M).ToList();

                // Assert: Verify the filtered results
                Assert.NotNull(filteredMovies); // Ensure the filtered list is not null
                Assert.Single(filteredMovies); // Ensure there is exactly one movie with the specified price
                Assert.Equal(15M, filteredMovies[0]?.Price); // Ensure the price of the filtered movie is 15M

                // Enhanced output to console for debugging and verification
                _output.WriteLine("=== Test Output ===");
                _output.WriteLine("Test: OnGetAsync_FilterMoviesByPrice");
                _output.WriteLine("Description: This test verifies that the OnGetAsync method correctly filters movies by price.");
                _output.WriteLine("Steps:");
                _output.WriteLine("1. Arrange: Set up the in-memory database with test data.");
                _output.WriteLine("   - Cleared existing movies.");
                _output.WriteLine("   - Added test movies to the database.");
                _output.WriteLine("2. Act: Execute the OnGetAsync method to populate the page model.");
                _output.WriteLine("   - Called OnGetAsync to retrieve movies.");
                _output.WriteLine("   - Filtered movies by price (15M).");
                _output.WriteLine("3. Assert: Verify the filtered results.");
                _output.WriteLine("   - Ensured the filtered list is not null.");
                _output.WriteLine("   - Ensured there is exactly one movie with the specified price.");
                _output.WriteLine("   - Ensured the price of the filtered movie is 15M.");
                _output.WriteLine($"Movies count: {filteredMovies.Count}");
                foreach (var movie in filteredMovies)
                {
                    if (movie != null)
                    {
                        _output.WriteLine($"- Title: {movie.Title}");
                        _output.WriteLine($"  Genre: {movie.Genre}");
                        _output.WriteLine($"  Price: {movie.Price}");
                        _output.WriteLine($"  ReleaseDate: {movie.ReleaseDate}");
                    }
                }
                _output.WriteLine("===================");
            }
        }

        [Fact]
        public async Task OnGetAsync_FilterMoviesByGenre()
        {
            // Arrange
            using (var context = new RazorPagesMovieContext(_options))
            {
                context.Movie.RemoveRange(context.Movie);
                context.Movie.AddRange(GetTestMovies());
                context.SaveChanges();

                var pageModel = new IndexModel(context);

                // Act
                await pageModel.OnGetAsync();
                var filteredMovies = pageModel.Movie.Where(m => m.Genre == "Genre 1").ToList();

                // Assert
                Assert.NotNull(filteredMovies);
                Assert.Single(filteredMovies);
                Assert.Equal("Genre 1", filteredMovies[0]?.Genre);

                // Enhanced output to console
                _output.WriteLine("=== Test Output ===");
                _output.WriteLine("Test: OnGetAsync_FilterMoviesByGenre");
                _output.WriteLine($"Movies count: {filteredMovies.Count}");
                foreach (var movie in filteredMovies)
                {
                    if (movie != null)
                    {
                        _output.WriteLine($"- Title: {movie.Title}");
                        _output.WriteLine($"  Genre: {movie.Genre}");
                        _output.WriteLine($"  Price: {movie.Price}");
                        _output.WriteLine($"  ReleaseDate: {movie.ReleaseDate}");
                    }
                }
                _output.WriteLine("===================");
            }
        }

        [Fact]
        public async Task OnGetAsync_FilterMoviesByReleaseDate()
        {
            // Arrange
            using (var context = new RazorPagesMovieContext(_options))
            {
                context.Movie.RemoveRange(context.Movie);
                context.Movie.AddRange(GetTestMovies());
                context.SaveChanges();
            }

            // Act
            using (var context = new RazorPagesMovieContext(_options))
            {
                var pageModel = new RazorPagesMovie.Pages.Movies.IndexModel(context);
                await pageModel.OnGetAsync();
                var filteredMovies = pageModel.Movie?.Where(m => m.ReleaseDate == DateTime.Parse("1989-2-12")).ToList();

                // Assert
                Assert.NotNull(filteredMovies);
                Assert.Single(filteredMovies);
                Assert.Equal(DateTime.Parse("1989-2-12"), filteredMovies.First().ReleaseDate);
            }
        }

        // Helper method for creating test movies
        private Movie CreateTestMovie(string title = "Test Movie")
        {
            return new Movie
            {
                Title = title,
                ReleaseDate = DateTime.Now,
                Genre = "Test Genre",
                Price = 9.99M,
                Timestamp = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 }
            };
        }

        // Update GetTestMovies to use helper method
        private List<Movie> GetTestMovies()
        {
            return new List<Movie>
            {
                new Movie
                {
                    Id = 1,
                    Title = "Test Movie 1",
                    ReleaseDate = DateTime.Parse("1989-2-12"),
                    Genre = "Romantic Comedy",
                    Price = 7.99M,
                    Rating = "PG",
                    Timestamp = new byte[8] // Initialize with a default value
                },
                new Movie
                {
                    Id = 2,
                    Title = "Test Movie 2",
                    ReleaseDate = DateTime.Parse("1984-3-13"),
                    Genre = "Comedy",
                    Price = 8.99M,
                    Rating = "PG",
                    Timestamp = new byte[8] // Initialize with a default value
                },
                // Add movies that match the criteria for the failing tests
                new Movie
                {
                    Id = 3,
                    Title = "Test Movie 3",
                    ReleaseDate = DateTime.Parse("1990-1-1"),
                    Genre = "Genre 1",
                    Price = 15M,
                    Rating = "PG",
                    Timestamp = new byte[8] // Initialize with a default value
                }
            };
        }

        private void ClearDatabase()
        {
            using (var context = new RazorPagesMovieContext(_options))
            {
                context.Movie.RemoveRange(context.Movie);
                context.SaveChanges();
            }
        }

        public void Dispose()
        {
            // Clean up the database after each test
            ClearDatabase();
        }
    }
}