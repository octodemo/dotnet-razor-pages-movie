using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Moq;
using RazorPagesMovie.Data;
using RazorPagesMovie.Models;
using Xunit;
using Xunit.Abstractions;
using RazorPagesMovie.Pages.Movies;

namespace RazorPagesMovie.Tests
{
    public class DeleteModelTests
    {
        private readonly Mock<RazorPagesMovieContext> _mockContext;
        private readonly DeleteModel _deleteModel;
        private readonly ITestOutputHelper _output;

        public DeleteModelTests(ITestOutputHelper output)
        {
            _output = output ?? throw new ArgumentNullException(nameof(output));
            _mockContext = new Mock<RazorPagesMovieContext>(new DbContextOptions<RazorPagesMovieContext>());
            _deleteModel = new DeleteModel(_mockContext.Object);
        }

        private DbContextOptions<RazorPagesMovieContext> GetContextOptions()
        {
            return new DbContextOptionsBuilder<RazorPagesMovieContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;
        }

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
                    Rating = "PG", // Add Rating
                    Timestamp = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 } // Initialize with a default value
                },
                new Movie
                {
                    Id = 2,
                    Title = "Test Movie 2",
                    ReleaseDate = DateTime.Parse("1984-3-13"),
                    Genre = "Comedy",
                    Price = 8.99M,
                    Rating = "PG", // Add Rating
                    Timestamp = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 } // Initialize with a default value
                }
            };
        }

        [Fact]
        public async Task CanDeleteMovie()
        {
            // Arrange
            var options = GetContextOptions();
            using (var context = new RazorPagesMovieContext(options))
            {
                var movie = new Movie 
                { 
                    Title = "Test Movie",
                    Genre = "Test Genre",
                    Price = 10M,
                    ReleaseDate = DateTime.Now,
                    Rating = "PG", // Add Rating
                    Timestamp = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 }  // Add Timestamp
                };
                
                _output.WriteLine("=== Test Output ===");
                _output.WriteLine("Test: CanDeleteMovie");
                _output.WriteLine("Creating test movie...");
                
                context.Movie.Add(movie);
                await context.SaveChangesAsync();
                
                _output.WriteLine($"Created movie: {movie.Title} (ID: {movie.Id})");
                _output.WriteLine($"Initial movie count: {await context.Movie.CountAsync()}");
            }

            using (var context = new RazorPagesMovieContext(options))
            {
                var movie = await context.Movie.FirstOrDefaultAsync();
                 Assert.NotNull(movie);
                                
                _output.WriteLine($"Found movie to delete: {movie?.Title ?? "Unknown title"}");
                if (movie == null)
                {
                    _output.WriteLine("Cannot delete: movie is null");
                    throw new InvalidOperationException("Cannot delete null movie");
                }
                
                context.Movie.Remove(movie);
                await context.SaveChangesAsync();
                _output.WriteLine($"Successfully deleted movie with ID: {movie.Id}");
                
                _output.WriteLine("Movie deleted");
            }

            using (var context = new RazorPagesMovieContext(options))
            {
                var count = await context.Movie.CountAsync();
                _output.WriteLine($"Final movie count: {count}");
                _output.WriteLine("===================");
                
                Assert.Equal(0, count);
            }
        }

        [Fact]
        // a method that creates a movie, then tries to delete a movie that doesn't exist in the database and check before and after the deletion - use _output.WriteLine with the same formatting as the other tests
        public async Task CanNotDeleteMovie()
        {
            // Arrange
            var options = GetContextOptions();
            using (var context = new RazorPagesMovieContext(options))
            {
                var movie = new Movie 
                { 
                    Title = "Test Movie", 
                    Genre = "Test Genre", 
                    Price = 10M, 
                    ReleaseDate = DateTime.Now,
                    Rating = "PG", // Add Rating
                    Timestamp = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 } // Add Timestamp
                };
                context.Movie.Add(movie);
                context.SaveChanges();
            }

            // Act
            using (var context = new RazorPagesMovieContext(options))
            {
                var movie = await context.Movie.FirstOrDefaultAsync(m => m.Title == "Test Movie");
                _output.WriteLine("=== Test Output ===");
                if (movie != null)
                {
                    _output.WriteLine($"Initial Movie: {movie.Title}");
                    _output.WriteLine("Rmoving movie from the database...");
                    context.Movie.Remove(movie);
                    await context.SaveChangesAsync();
                }
                else
                {
                    _output.WriteLine("Movie not found.");
                }

                _output.WriteLine("Assert.NotNull(movie);");
                Assert.NotNull(movie);
            }

            // Assert - check that the movie is deleted from the database
            using (var context = new RazorPagesMovieContext(options))
            {
                var movie = await context.Movie.FirstOrDefaultAsync(m => m.Title == "Test Movie");
                // convert movie object to string to print it
                string str_movie = movie?.ToString() ?? "null";
                _output.WriteLine($"Deleted Movie: {str_movie}");
                _output.WriteLine("Assert.Null(movie);");
                Assert.Null(movie);
                _output.WriteLine("Trying to fetch the movie by title: 'Test Movie' again, expected null");
                _output.WriteLine("Fetch by expression: context.Movie.FirstOrDefaultAsync(m => m.Title == 'Test Movie');");
                var movie2 = await context.Movie.FirstOrDefaultAsync(m => m.Title == "Test Movie");
                _output.WriteLine("Assert.Null(movie);");
                Assert.Null(movie2);
                _output.WriteLine("===================");
            }
        }

        [Fact]
        public async Task CanHandleDeadlock()
        {
            // Arrange
            var options = GetContextOptions();
            using (var context = new RazorPagesMovieContext(options))
            {
                var movie = new Movie
                {
                    Title = "Test Movie",
                    Genre = "Test Genre",
                    Price = 10M,
                    ReleaseDate = DateTime.Now,
                    Rating = "PG",
                    Timestamp = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 } // Add Timestamp
                };

                _output.WriteLine("=== Test Output ===");
                _output.WriteLine("Creating test movie for deadlock test");
                
                context.Movie.Add(movie);
                await context.SaveChangesAsync();
                
                _output.WriteLine($"Created movie: {movie.Title} (ID: {movie.Id})");
            }

            // Act - Simulate concurrent operations
            using (var context1 = new RazorPagesMovieContext(options))
            using (var context2 = new RazorPagesMovieContext(options))
            {
                var movie1 = await context1.Movie.FirstAsync();
                var movie2 = await context2.Movie.FirstAsync();

                _output.WriteLine("Attempting concurrent updates");

                movie1.Title = "Updated Title 1";
                movie2.Title = "Updated Title 2";

                await context1.SaveChangesAsync();
                
                try
                {
                    await context2.SaveChangesAsync();
                    _output.WriteLine("Second save succeeded unexpectedly");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    _output.WriteLine("Expected concurrency exception caught");
                    Assert.Contains("Database operation expected to affect 1 row(s)", ex.Message);
                }

                _output.WriteLine("===================");
            }
        }
    }
}
