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
            _mockContext = new Mock<RazorPagesMovieContext>(new DbContextOptions<RazorPagesMovieContext>());
            _deleteModel = new DeleteModel(_mockContext.Object);
            _output = output;
        }

        private DbContextOptions<RazorPagesMovieContext> GetContextOptions()
        {
            return new DbContextOptionsBuilder<RazorPagesMovieContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        // a method that creates a movie, then deletes it from the database and check before and after the deletion - use _output.WriteLine with the same formatting as the other tests
        public async Task CanDeleteMovie()
        {
            // Arrange
            var options = GetContextOptions();
            using (var context = new RazorPagesMovieContext(options))
            {
                context.Movie.Add(new Movie { Title = "Test Movie", Genre = "Test Genre", Price = 10M, ReleaseDate = DateTime.Now });
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
                _output.WriteLine("===================");
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
                context.Movie.Add(new Movie { Title = "Test Movie", Genre = "Test Genre", Price = 10M, ReleaseDate = DateTime.Now });
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
                context.Movie.Add(new Movie { Title = "Test Movie", Genre = "Test Genre", Price = 10M, ReleaseDate = DateTime.Now });
                context.SaveChanges();
            }
        
            // Act
            _output.WriteLine($"=== Test Output: ===");
            var tasks = new List<Task>();
            for (int i = 0; i < 10; i++)
            {
                int iteration = i + 1;
                tasks.Add(Task.Run(async () =>
                {
                    using (var context = new RazorPagesMovieContext(options))
                    {
                        var movie = await context.Movie.FirstOrDefaultAsync(m => m.Title == "Test Movie");
                        if (movie != null)
                        {
                            _output.WriteLine($"Initial Movie: {movie.Title} (Iteration: {iteration})");
                            _output.WriteLine("Removing movie from the database...");
                            context.Movie.Remove(movie);
                            try
                            {
                                await context.SaveChangesAsync();
                                _output.WriteLine($"Movie removed successfully (Iteration: {iteration}).");
                            }
                            catch (DbUpdateConcurrencyException)
                            {
                                _output.WriteLine($"Concurrency exception occurred (Iteration: {iteration}).");
                            }
                        }
                        else
                        {
                            _output.WriteLine($"Movie not found (Iteration: {iteration}).");
                        }
                    }
                }));
            }
        
            await Task.WhenAll(tasks);
        
            // Assert - check that the movie is deleted from the database
            using (var context = new RazorPagesMovieContext(options))
            {
                var movie = await context.Movie.FirstOrDefaultAsync(m => m.Title == "Test Movie");
                string str_movie = movie?.ToString() ?? "null";
                _output.WriteLine($"Deleted Movie: {str_movie}");
                _output.WriteLine("Assert.Null(movie);");
                Assert.Null(movie);
                _output.WriteLine("===================");
            }
        }
    }
}
