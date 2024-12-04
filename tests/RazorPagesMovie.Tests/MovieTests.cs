using System;
using Xunit;
using RazorPagesMovie.Models;
using Xunit.Abstractions;

namespace RazorPagesMovie.Tests
{
    public class MovieTests
    {
        private readonly ITestOutputHelper _output;

        public MovieTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void CanChangeTitle()
        {
            // Arrange
            var movie = new Movie { Title = "Old Title" };
            _output.WriteLine("=== Test Output ===");
            _output.WriteLine($"Initial Title: {movie.Title}");

            // Act
            movie.Title = "New Title";
            _output.WriteLine($"Updated Title: {movie.Title}");
            _output.WriteLine("===================");

            // Assert
            Assert.Equal("New Title", movie.Title);
            Assert.NotEqual("Old Title", movie.Title);
            Assert.False(string.IsNullOrEmpty(movie.Title));
        }

        [Fact]
        public void CanChangeGenre()
        {
            // Arrange
            var movie = new Movie { Genre = "Old Genre" };
            _output.WriteLine("=== Test Output ===");
            _output.WriteLine($"Initial Genre: {movie.Genre}");

            // Act
            movie.Genre = "New Genre";
            _output.WriteLine($"Updated Genre: {movie.Genre}");
            _output.WriteLine("===================");

            // Assert
            Assert.Equal("New Genre", movie.Genre);
            Assert.NotEqual("Old Genre", movie.Genre);
            Assert.False(string.IsNullOrEmpty(movie.Genre));
        }

        [Fact]
        public void CanChangePrice()
        {
            // Arrange
            var movie = new Movie { Price = 10M };
            _output.WriteLine("=== Test Output ===");
            _output.WriteLine($"Initial Price: {movie.Price}");

            // Act
            movie.Price = 15M;
            _output.WriteLine($"Updated Price: {movie.Price}");
            _output.WriteLine("===================");

            // Assert
            Assert.Equal(15M, movie.Price);
            Assert.NotEqual(10M, movie.Price);
            Assert.True(movie.Price > 0);
        }

        [Fact]
        public void CanChangeReleaseDate()
        {
            // Arrange
            var movie = new Movie { ReleaseDate = DateTime.Parse("2023-01-01") };
            _output.WriteLine("=== Test Output ===");
            _output.WriteLine($"Initial Release Date: {movie.ReleaseDate}");

            // Act
            movie.ReleaseDate = DateTime.Parse("2023-12-31");
            _output.WriteLine($"Updated Release Date: {movie.ReleaseDate}");
            _output.WriteLine("===================");

            // Assert
            Assert.Equal(DateTime.Parse("2023-12-31"), movie.ReleaseDate);
            Assert.NotEqual(DateTime.Parse("2023-01-01"), movie.ReleaseDate);
            Assert.True(movie.ReleaseDate > DateTime.MinValue);
        }

        [Theory]
        [InlineData("Action")]
        [InlineData("Comedy")]
        [InlineData("Drama")]
        public void CanChangeGenreWithDifferentValues(string newGenre)
        {
            // Arrange
            var movie = new Movie { Genre = "Old Genre" };
            _output.WriteLine("=== Test Output ===");
            _output.WriteLine($"Initial Genre: {movie.Genre}");

            // Act
            movie.Genre = newGenre;
            _output.WriteLine($"Updated Genre: {movie.Genre}");
            _output.WriteLine("===================");

            // Assert
            Assert.Equal(newGenre, movie.Genre);
            Assert.NotEqual("Old Genre", movie.Genre);
            Assert.False(string.IsNullOrEmpty(movie.Genre));
        }

        [Theory]
        [InlineData(5.99)]
        [InlineData(15.00)]
        [InlineData(20.50)]
        public void CanChangePriceWithDifferentValues(decimal newPrice)
        {
            // Arrange
            var movie = new Movie { Price = 10M };
            _output.WriteLine("=== Test Output ===");
            _output.WriteLine($"Initial Price: {movie.Price}");

            // Act
            movie.Price = newPrice;
            _output.WriteLine($"Updated Price: {movie.Price}");
            _output.WriteLine("===================");

            // Assert
            Assert.Equal(newPrice, movie.Price);
            Assert.NotEqual(10M, movie.Price);
            Assert.True(movie.Price > 0);
        }

        [Theory]
        [InlineData("2023-01-01", "2023-12-31")]
        [InlineData("2022-05-15", "2022-11-20")]
        [InlineData("2021-07-07", "2021-08-08")]
        public void CanChangeReleaseDateWithDifferentValues(string initialDate, string newDate)
        {
            // Arrange
            var movie = new Movie { ReleaseDate = DateTime.Parse(initialDate) };
            _output.WriteLine("=== Test Output ===");
            _output.WriteLine($"Initial Release Date: {movie.ReleaseDate}");

            // Act
            movie.ReleaseDate = DateTime.Parse(newDate);
            _output.WriteLine($"Updated Release Date: {movie.ReleaseDate}");
            _output.WriteLine("===================");

            // Assert
            Assert.Equal(DateTime.Parse(newDate), movie.ReleaseDate);
            Assert.NotEqual(DateTime.Parse(initialDate), movie.ReleaseDate);
            Assert.True(movie.ReleaseDate > DateTime.MinValue);
        }
    }
}