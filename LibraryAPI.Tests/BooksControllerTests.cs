using AutoMapper;
using LibraryAPI.Controllers;
using LibraryAPI.DTOs;
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
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IImageService> _mockImageService;
        private readonly Mock<INotificationService> _mockNotificationService;
        private readonly BooksController _controller;

        public BooksControllerTests()
        {
            _mockService = new Mock<IBookService>();
            _mockMapper = new Mock<IMapper>();
            _mockImageService = new Mock<IImageService>();
            _mockNotificationService = new Mock<INotificationService>();
            
            _controller = new BooksController(
                _mockService.Object,
                _mockImageService.Object,
                _mockNotificationService.Object,
                _mockMapper.Object);
        }

        [Fact]
        public async Task GetBook_ReturnsBook_WhenExists()
        {
            var book = new Book 
            { 
                Id = 1, 
                Title = "1984", 
                Genre = "Dystopia", 
                ISBN = "123", 
                Description = "desc", 
                AuthorId = 1 
            };
            var bookDTO = new Book 
            { 
                Id = 1, 
                Title = "1984", 
                Genre = "Dystopia", 
                ISBN = "123", 
                Description = "desc", 
                AuthorId = 1 
            };

            _mockService.Setup(s => s.GetBookByIdAsync(1)).ReturnsAsync(book);
            _mockMapper.Setup(m => m.Map<Book>(It.IsAny<Book>())).Returns(bookDTO);
            
            var result = await _controller.GetBook(1);
            
            var actionResult = Assert.IsType<ActionResult<Book>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Equal(bookDTO, okResult.Value);
        }

        [Fact]
        public async Task CreateBook_ReturnsCreatedResponse()
        {
            var createBookDto = new CreateBookDTO
            {
                Title = "Brave New World",
                ISBN = "123-4567890123",
                Genre = "Dystopian",
                Description = "A novel by Aldous Huxley",
                AuthorId = 1
            };

            var book = new Book
            {
                Id = 1,
                Title = createBookDto.Title,
                ISBN = createBookDto.ISBN,
                Genre = createBookDto.Genre,
                Description = createBookDto.Description,
                AuthorId = createBookDto.AuthorId
            };

            var bookDTO = new Book
            {
                Id = book.Id,
                Title = book.Title,
                ISBN = book.ISBN,
                Genre = book.Genre,
                Description = book.Description,
                AuthorId = book.AuthorId
            };
            
            _mockMapper.Setup(m => m.Map<Book>(It.IsAny<CreateBookDTO>())).Returns(book);
            _mockMapper.Setup(m => m.Map<Book>(It.IsAny<Book>())).Returns(bookDTO);
            _mockService.Setup(s => s.AddBookAsync(It.IsAny<Book>())).Returns(Task.CompletedTask);
            
            var result = await _controller.CreateBook(createBookDto);
            
            var actionResult = Assert.IsType<ActionResult<Book>>(result);
            var createdAtAction = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var returnedBookDto = Assert.IsType<Book>(createdAtAction.Value);

            Assert.Equal(bookDTO.Id, returnedBookDto.Id);
            Assert.Equal(bookDTO.Title, returnedBookDto.Title);
            Assert.Equal(bookDTO.ISBN, returnedBookDto.ISBN);
        }
    }
}
