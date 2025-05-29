using System;
using Xunit;
using RazorPagesMovie.Models;
using Xunit.Abstractions;

namespace RazorPagesMovie.Tests
{
    public class ArtistTests
    {
        private readonly ITestOutputHelper _output;

        public ArtistTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void CanChangeName()
        {
            // Arrange
            var artist = new Artist { Name = "Old Name" };
            _output.WriteLine("=== Test Output ===");
            _output.WriteLine($"Initial Name: {artist.Name}");

            // Act
            artist.Name = "New Name";
            _output.WriteLine($"Updated Name: {artist.Name}");
            _output.WriteLine("===================");

            // Assert
            Assert.Equal("New Name", artist.Name);
            Assert.NotEqual("Old Name", artist.Name);
            Assert.False(string.IsNullOrEmpty(artist.Name));
        }

        [Fact]
        public void CanChangeBirthDate()
        {
            // Arrange
            var artist = new Artist { BirthDate = DateTime.Parse("1980-01-01") };
            _output.WriteLine("=== Test Output ===");
            _output.WriteLine($"Initial Birth Date: {artist.BirthDate}");

            // Act
            artist.BirthDate = DateTime.Parse("1985-12-31");
            _output.WriteLine($"Updated Birth Date: {artist.BirthDate}");
            _output.WriteLine("===================");

            // Assert
            Assert.Equal(DateTime.Parse("1985-12-31"), artist.BirthDate);
            Assert.NotEqual(DateTime.Parse("1980-01-01"), artist.BirthDate);
            Assert.True(artist.BirthDate > DateTime.MinValue);
        }

        [Fact]
        public void CanChangeNationality()
        {
            // Arrange
            var artist = new Artist { Nationality = "American" };
            _output.WriteLine("=== Test Output ===");
            _output.WriteLine($"Initial Nationality: {artist.Nationality}");

            // Act
            artist.Nationality = "British";
            _output.WriteLine($"Updated Nationality: {artist.Nationality}");
            _output.WriteLine("===================");

            // Assert
            Assert.Equal("British", artist.Nationality);
            Assert.NotEqual("American", artist.Nationality);
        }

        [Theory]
        [InlineData("Leonardo DiCaprio")]
        [InlineData("Meryl Streep")]
        [InlineData("Robert De Niro")]
        public void CanChangeNameWithDifferentValues(string newName)
        {
            // Arrange
            var artist = new Artist { Name = "Old Name" };
            _output.WriteLine("=== Test Output ===");
            _output.WriteLine($"Initial Name: {artist.Name}");

            // Act
            artist.Name = newName;
            _output.WriteLine($"Updated Name: {artist.Name}");
            _output.WriteLine("===================");

            // Assert
            Assert.Equal(newName, artist.Name);
            Assert.NotEqual("Old Name", artist.Name);
            Assert.False(string.IsNullOrEmpty(artist.Name));
        }

        [Theory]
        [InlineData("American")]
        [InlineData("British")]
        [InlineData("French")]
        [InlineData(null)]
        public void CanChangeNationalityWithDifferentValues(string? newNationality)
        {
            // Arrange
            var artist = new Artist { Nationality = "German" };
            _output.WriteLine("=== Test Output ===");
            _output.WriteLine($"Initial Nationality: {artist.Nationality}");

            // Act
            artist.Nationality = newNationality;
            _output.WriteLine($"Updated Nationality: {artist.Nationality}");
            _output.WriteLine("===================");

            // Assert
            Assert.Equal(newNationality, artist.Nationality);
            Assert.NotEqual("German", artist.Nationality);
        }
    }
}