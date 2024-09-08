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
                Price = 10.99M
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
            _output.WriteLine("=== Test Output ===");
            _output.WriteLine("Creating a mock DetailsModel object and calling OnGetAsync with a movie and an invalid id:");
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
                Price = 10.99M
            };
            context.Movie.Add(movie);
            context.SaveChanges();

            // Act
            // Call OnGetAsync with an invalid id
            var result = await detailsModel.OnGetAsync(2);
            // Assert
            // Assert that the result is invalid id
            _output.WriteLine("a. Asserting that the result is of type NotFoundResult:");
            _output.WriteLine("   Assert.IsType<NotFoundResult>(result);");
            // Assert that the result is of type NotFoundResult
            Assert.IsType<NotFoundResult>(result);
            _output.WriteLine("b. Asserting that the result is of type NotFoundResult:");
            _output.WriteLine("   Assert.IsType<NotFoundResult>(result);");
            _output.WriteLine("===================");
        }

        [Fact]
        public async Task DetailsModel_OnGetAsync_ReturnsPageResult_WhenMovieIsNotNullAndIdIsValid()
        {
            _output.WriteLine("=== Test Output ===");
            _output.WriteLine("Creating a mock DetailsModel object and calling OnGetAsync with a movie and a valid id:");
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
                Price = 10.99M
            };
            context.Movie.Add(movie);
            context.SaveChanges();

            // Act
            // Call OnGetAsync with a valid id
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
            // Assert that the movie is not null
            Assert.NotNull(detailsModel.Movie);
            _output.WriteLine("c. Asserting that the movie is not null:");
            _output.WriteLine("   Assert.NotNull(detailsModel.Movie);");
            // Assert that the movie is of type Movie
            Assert.IsType<Movie>(detailsModel.Movie);
            _output.WriteLine("d. Asserting that the movie is of type Movie:");
            _output.WriteLine("   Assert.IsType<Movie>(detailsModel.Movie);");
            // Assert that the movie has the correct id
            Assert.Equal(1, detailsModel.Movie.Id);
            _output.WriteLine("e. Asserting that the movie has the correct id:");
            _output.WriteLine("   Assert.Equal(1, detailsModel.Movie.Id);");
            // Assert that the movie has the correct title
            Assert.Equal("Inception", detailsModel.Movie.Title);
            _output.WriteLine("f. Asserting that the movie has the correct title:");
            _output.WriteLine("   Assert.Equal(\"Inception\", detailsModel.Movie.Title);");
            // Assert that the movie has the correct release date
            Assert.Equal(DateTime.Parse("2010-07-16"), detailsModel.Movie.ReleaseDate);
            _output.WriteLine("g. Asserting that the movie has the correct release date:");
            _output.WriteLine("   Assert.Equal(DateTime.Parse(\"2010-07-16\"), detailsModel.Movie.ReleaseDate);");
            // Assert that the movie has the correct genre
            Assert.Equal("Sci-Fi", detailsModel.Movie.Genre);
            _output.WriteLine("h. Asserting that the movie has the correct genre:");
            _output.WriteLine("   Assert.Equal(\"Sci-Fi\", detailsModel.Movie.Genre);");
            // Assert that the movie has the correct price
            Assert.Equal(10.99M, detailsModel.Movie.Price);
            _output.WriteLine("i. Asserting that the movie has the correct price:");
            _output.WriteLine("   Assert.Equal(10.99M, detailsModel.Movie.Price);");
            _output.WriteLine("===================");
        }
    }
}