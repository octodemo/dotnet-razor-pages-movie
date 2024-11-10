using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using RazorPagesMovie.Models;
using RazorPagesMovie.Data;
using RazorPagesMovie.Pages.Movies;
using Xunit.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace RazorPagesMovie.Tests
{
    public class DetailsModelTests
    {
        private readonly ITestOutputHelper _output;

        public DetailsModelTests(ITestOutputHelper output)
        {
            _output = output;
        }

        private DbContextOptions<RazorPagesMovieContext> CreateNewContextOptions()
        {
            return new DbContextOptionsBuilder<RazorPagesMovieContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public void DetailsModel_CanBeInstantiated()
        {
            // Arrange
            var options = CreateNewContextOptions();
            var mockLogger = new Mock<ILogger<DetailsModel>>();
            var context = new RazorPagesMovieContext(options);
            _output.WriteLine("=== Test Output ===");
            _output.WriteLine("Creating a mock DetailsModel object and asserting that it is not null:");
            // Act
            var detailsModel = new DetailsModel(context, mockLogger.Object);
            //  Assert that the model is not null
            Assert.NotNull(detailsModel);
            _output.WriteLine("a. Asserting that the model is not null:");
            _output.WriteLine("   Assert.NotNull(model);");
            _output.WriteLine("b. Asserting that the model is of type PageModel:");
            _output.WriteLine("   Assert.IsType<PageModel>(model);");
            Assert.IsType<DetailsModel>(detailsModel);
            _output.WriteLine("===================");
        }

        [Fact]
        public void DetailsModel_OnGetAsync_ReturnsNotFoundResult_WhenIdIsNull()
        {
            _output.WriteLine("=== Test Output ===");
            _output.WriteLine("Creating a mock DetailsModel object and calling OnGetAsync with a null id:");
            // Arrange
            var options = CreateNewContextOptions();
            var mockLogger = new Mock<ILogger<DetailsModel>>();
            var context = new RazorPagesMovieContext(options);
            var detailsModel = new DetailsModel(context, mockLogger.Object);
            // Act
            var result = detailsModel.OnGetAsync(null).Result;
            // Assert
            // Assert that the result is of type NotFoundResult
            Assert.IsType<NotFoundResult>(result);
            _output.WriteLine("a. Asserting that the result is of type NotFoundResult:");
            _output.WriteLine("   Assert.IsType<NotFoundResult>(result);");
            // Assert that the result is of type NotFoundResult
            Assert.IsType<NotFoundResult>(result);
            _output.WriteLine("b. Asserting that the result is of type NotFoundResult:");
            _output.WriteLine("   Assert.IsType<NotFoundResult>(result);");
            _output.WriteLine("===================");
        }

        [Fact]
        public async Task DetailsModel_OnGetAsync_ReturnsNotFoundResult_WhenMovieIsNull()
        {
            _output.WriteLine("=== Test Output ===");
            _output.WriteLine("Creating a mock DetailsModel object and calling OnGetAsync with a null movie:");
            // Arrange
            var options = CreateNewContextOptions();
            var mockLogger = new Mock<ILogger<DetailsModel>>();
            var context = new RazorPagesMovieContext(options);
            var detailsModel = new DetailsModel(context, mockLogger.Object);
            // Act
            var result = await detailsModel.OnGetAsync(1);
            // Assert
            // Assert that the result is of type NotFoundResult
            Assert.IsType<NotFoundResult>(result);
            _output.WriteLine("a. Asserting that the result is of type NotFoundResult:");
            _output.WriteLine("   Assert.IsType<NotFoundResult>(result);");
            // Assert that the result is of type NotFoundResult
            Assert.IsType<NotFoundResult>(result);
            _output.WriteLine("b. Asserting that the result is of type NotFoundResult:");
            _output.WriteLine("   Assert.IsType<NotFoundResult>(result);");
            _output.WriteLine("===================");
        }

        [Fact]
        public async Task DetailsModel_OnGetAsync_ReturnsPageResult_WhenMovieIsNotNull()
        {
            _output.WriteLine("=== Test Output ===");
            _output.WriteLine("Creating a mock DetailsModel object and calling OnGetAsync with a movie:");
            // Arrange
            var options = CreateNewContextOptions();
            var mockLogger = new Mock<ILogger<DetailsModel>>();
            var context = new RazorPagesMovieContext(options);
            var detailsModel = new DetailsModel(context, mockLogger.Object);
            var movie = new Movie
            {
                Id = 1,
                Title = "Inception",
                ReleaseDate = DateTime.Parse("2010-07-16"),
                Genre = "Sci-Fi",
                Price = 10.99M,
                Timestamp = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 } // Add Timestamp
            };
            context.Movie.Add(movie);
            context.SaveChanges();

            // Act
            var result = await detailsModel.OnGetAsync(1);
            // Assert
            // Assert that the result is of type PageResult
            Assert.IsType<PageResult>(result);
            _output.WriteLine("a. Asserting that the result is of type PageResult:");
            _output.WriteLine("   Assert.IsType<PageResult>(result);");
            // Assert that the result is of type PageResult
            Assert.IsType<PageResult>(result);
            _output.WriteLine("b. Asserting that the result is of type PageResult:");
            _output.WriteLine("   Assert.IsType<PageResult>(result);");
            _output.WriteLine("===================");
        }

        [Fact]
        public async Task DetailsModel_OnGetAsync_ReturnsPageResult_WhenMovieIsNotNullAndIdIsNotValid()
        {
            // Arrange
            var options = CreateNewContextOptions();
            var mockLogger = new Mock<ILogger<DetailsModel>>();
            var context = new RazorPagesMovieContext(options);
            var detailsModel = new DetailsModel(context, mockLogger.Object);

            var movie = new Movie
            {
                Id = 1,
                Title = "Test Movie",
                ReleaseDate = DateTime.Now,
                Genre = "Test Genre",
                Price = 9.99M,
                Timestamp = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 } // Add Timestamp
            };

            _output.WriteLine("=== Test Output ===");
            _output.WriteLine("Creating a mock DetailsModel object and calling OnGetAsync with a movie and an invalid id:");
            
            context.Movie.Add(movie);
            context.SaveChanges();

            // Act
            var result = await detailsModel.OnGetAsync(2); // Invalid ID

            // Assert
            Assert.IsType<NotFoundResult>(result);
            
            _output.WriteLine($"Movie added with ID: {movie.Id}");
            _output.WriteLine($"Attempted to retrieve movie with invalid ID: 2");
            _output.WriteLine($"Result type: {result.GetType().Name}");
            _output.WriteLine("===================");
        }

        [Fact]
        public async Task DetailsModel_OnGetAsync_ReturnsPageResult_WhenMovieIsNotNullAndIdIsValid()
        {
            // Arrange
            var options = CreateNewContextOptions();
            var mockLogger = new Mock<ILogger<DetailsModel>>();
            var context = new RazorPagesMovieContext(options);
            var detailsModel = new DetailsModel(context, mockLogger.Object);

            var movie = new Movie
            {
                Id = 1,
                Title = "Test Movie",
                ReleaseDate = DateTime.Now,
                Genre = "Test Genre",
                Price = 9.99M,
                Timestamp = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 }  // Add Timestamp
            };

            _output.WriteLine("=== Test Output ===");
            _output.WriteLine("Creating test movie with valid ID");
            _output.WriteLine($"Movie ID: {movie.Id}");
            _output.WriteLine($"Movie Title: {movie.Title}");

            context.Movie.Add(movie);
            await context.SaveChangesAsync();

            // Act
            var result = await detailsModel.OnGetAsync(1);  // Use valid ID

            // Assert
            _output.WriteLine("Verifying results:");
            Assert.IsType<PageResult>(result);
            Assert.NotNull(detailsModel.Movie);
            Assert.Equal(movie.Title, detailsModel.Movie.Title);
            
            _output.WriteLine($"Result type: {result.GetType().Name}");
            _output.WriteLine($"Movie found: {detailsModel.Movie?.Title}");
            _output.WriteLine("===================");
        }
    }
}