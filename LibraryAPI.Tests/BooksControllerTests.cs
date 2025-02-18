using LibraryAPI.Controllers;
using LibraryAPI.Models;
using LibraryAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LibraryAPI.Tests
{
    public class BooksControllerTests
    {
        private readonly Mock<IBookService> _mockService;
        private readonly BooksController _controller;

        public BooksControllerTests()
        {
            _mockService = new Mock<IBookService>();
            _controller = new BooksController(_mockService.Object, Mock.Of<IImageService>(), Mock.Of<INotificationService>());
        }

        [Fact]
        public async Task GetBook_ReturnsBook_WhenExists()
        {
            // Arrange
            var book = new Book { Id = 1, Title = "1984" };
            _mockService.Setup(s => s.GetBookByIdAsync(1)).ReturnsAsync(book);

            // Act
            var result = await _controller.GetBook(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Book>>(result);
            var returnValue = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Equal(book, returnValue.Value);
        }

        [Fact]
        public async Task CreateBook_ReturnsCreatedResponse()
        {
            // Arrange
            var book = new Book { Id = 1, Title = "Brave New World" };
            _mockService.Setup(s => s.AddBookAsync(book)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateBook(book);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Book>>(result);
            var createdAtAction = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            Assert.Equal(book, createdAtAction.Value);
        }
    }
}