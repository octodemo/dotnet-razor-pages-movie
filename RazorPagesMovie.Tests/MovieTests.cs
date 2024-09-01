using Xunit;
using RazorPagesMovie.Models;

namespace RazorPagesMovie.Tests
{
    public class UnitTest2
    {
        [Fact]
        public void Test1()
        {
            // write a test about the Movie class here - for example, we'll test that we can change the title
            var movie = new Movie { Title = "Old Title" };
            movie.Title = "New Title";
            Assert.Equal("New Title", movie.Title);
            // assert not equal
            Assert.NotEqual("Old Title", movie.Title);
        }
    }
}