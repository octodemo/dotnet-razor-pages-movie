using Microsoft.Extensions.Logging;
using RazorPagesMovie.Pages.Gallery;
using Xunit;
using Moq;

namespace RazorPagesMovie.Tests
{
    /// <summary>
    /// Test class for the Gallery page functionality
    /// </summary>
    public class GalleryPageTests
    {
        [Fact]
        public void GalleryPage_OnGet_Succeeds()
        {
            // Arrange: Create a mock logger
            var loggerMock = new Mock<ILogger<IndexModel>>();
            var galleryModel = new IndexModel(loggerMock.Object);

            // Act: Call the OnGet method
            galleryModel.OnGet();

            // Assert: Verify the model is initialized
            Assert.NotNull(galleryModel);
        }

        [Fact]
        public void GalleryPage_OnGet_WritesToLog()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<IndexModel>>();
            var galleryModel = new IndexModel(loggerMock.Object);

            // Act
            galleryModel.OnGet();

            // Assert: Check that logging occurred
            loggerMock.Verify(
                logger => logger.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((state, type) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((formatter, type) => true)),
                Times.Once);
        }
    }
}
