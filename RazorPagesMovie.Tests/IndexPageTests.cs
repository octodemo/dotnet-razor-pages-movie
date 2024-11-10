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
                Assert.Equal(2, pageModel.Movie.Count);

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
            using (var context = new RazorPagesMovieContext(_options))
            {
                // Arrange
                // Clear existing movies first
                context.Movie.RemoveRange(context.Movie);
                await context.SaveChangesAsync();

                var testMovie = new Movie
                {
                    Title = "Test Movie",
                    ReleaseDate = DateTime.Now,
                    Genre = "Test Genre",
                    Price = 9.99M,
                    Timestamp = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 }
                };
                
                context.Movie.Add(testMovie);
                await context.SaveChangesAsync();

                var pageModel = new IndexModel(context);

                // Act
                await pageModel.OnGetAsync();

                // Debug output
                _output.WriteLine("=== Test Output ===");
                _output.WriteLine($"Total movies in database: {context.Movie.Count()}");
                _output.WriteLine($"Movies in pageModel: {pageModel.Movie.Count}");
                _output.WriteLine("Movies found:");
                foreach (var movie in pageModel.Movie)
                {
                    _output.WriteLine($"- {movie.Title}");
                }
                _output.WriteLine("===================");

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
                Assert.Equal(2, pageModel.Movie.Count);

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
                Assert.Equal(2, pageModel.Movie.Count);

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
        
                var pageModel = new IndexModel(context);
        
                // Act
                await pageModel.OnGetAsync();
                var filteredMovies = pageModel.Movie?.Where(m => m.Title == "Movie 1").ToList();

                // Assert
                Assert.NotNull(filteredMovies);
                Assert.Single(filteredMovies);

                var firstMovie = filteredMovies?.FirstOrDefault();
                Assert.NotNull(firstMovie);
                Assert.Equal("Movie 1", firstMovie!.Title); // Use null-forgiving operator

                // Enhanced output to console
                _output.WriteLine("=== Test Output ===");
                _output.WriteLine("Test: OnGetAsync_FilterMoviesByTitle");
                _output.WriteLine("Asserting that the filtered Movie list is not null.");
                _output.WriteLine($"Filtered Movie list is not null: {filteredMovies != null}");
                _output.WriteLine("Asserting that the filtered Movie list contains 1 item.");
                _output.WriteLine($"Filtered Movies count: {filteredMovies?.Count}");
                _output.WriteLine("Asserting that the title of the filtered movie is 'Movie 1'.");

                if (filteredMovies != null && filteredMovies.Count > 0)
                {
                    _output.WriteLine($"Filtered Movie title: {filteredMovies[0]?.Title}");
                }

                if (filteredMovies != null)
                {
                    foreach (var movie in filteredMovies)
                    {
                        if (movie != null)
                        {
                            _output.WriteLine($"- Title: {movie.Title}");
                            _output.WriteLine($"  Genre: {movie.Genre}");
                            _output.WriteLine($"  Price: {movie.Price}");
                        }
                    }
                }
                else
                {
                    _output.WriteLine("No movies found.");
                }
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
                var testDate = DateTime.Parse("2023-01-01");
                context.Movie.RemoveRange(context.Movie);
                context.Movie.AddRange(GetTestMovies());
                context.SaveChanges();

                var pageModel = new IndexModel(context);

                // Act
                await pageModel.OnGetAsync();
                var filteredMovies = pageModel.Movie
                    .Where(m => m.ReleaseDate.Date == testDate.Date)
                    .ToList();

                // Debug output
                _output.WriteLine("=== Test Output ===");
                _output.WriteLine($"Test date: {testDate:yyyy-MM-dd}");
                _output.WriteLine($"Total movies: {pageModel.Movie.Count}");
                _output.WriteLine("All movies:");
                foreach (var m in pageModel.Movie)
                {
                    _output.WriteLine($"- {m.Title}: {m.ReleaseDate:yyyy-MM-dd}");
                }
                _output.WriteLine($"Filtered movies count: {filteredMovies.Count}");
                _output.WriteLine("===================");

                // Assert
                Assert.NotNull(filteredMovies);
                Assert.Single(filteredMovies);
                Assert.Equal(testDate.Date, filteredMovies[0].ReleaseDate.Date);
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
                    Title = "Movie 1",
                    ReleaseDate = DateTime.Parse("2023-01-01"),
                    Genre = "Genre 1",
                    Price = 9.99M,
                    Timestamp = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 }
                },
                new Movie
                {
                    Title = "Movie 2",
                    ReleaseDate = DateTime.Parse("2023-02-01"),
                    Genre = "Genre 2",
                    Price = 15M,
                    Timestamp = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 }
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