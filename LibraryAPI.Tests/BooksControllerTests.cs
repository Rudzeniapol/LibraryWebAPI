﻿using AutoMapper;
using LibraryAPI.API.Controllers;
using LibraryAPI.Application.DTOs;
using LibraryAPI.Domain.Models;
using LibraryAPI.Application.Services.Interfaces;
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
            _mockImageService = new Mock<IImageService>();
            _mockNotificationService = new Mock<INotificationService>();
            
            _controller = new BooksController(
                _mockService.Object,
                _mockImageService.Object,
                _mockNotificationService.Object);
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

            _mockService.Setup(s => s.GetBookByIdAsync(1, CancellationToken.None)).ReturnsAsync(book);
            _mockMapper.Setup(m => m.Map<Book>(It.IsAny<Book>())).Returns(bookDTO);
            
            var result = await _controller.GetBook(1, CancellationToken.None);
            
            var actionResult = Assert.IsType<ActionResult<Book>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Equal(bookDTO, okResult.Value);
        }

        [Fact]
        public async Task CreateBook_ReturnsCreatedResponse()
        {
            var createBookDto = new BookDTO
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
                Description = createBookDto.Description
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
            
            _mockMapper.Setup(m => m.Map<Book>(It.IsAny<BookDTO>())).Returns(book);
            _mockMapper.Setup(m => m.Map<Book>(It.IsAny<Book>())).Returns(bookDTO);
            _mockService.Setup(s => s.AddBookAsync(It.IsAny<BookDTO>(), CancellationToken.None)).Returns(Task.CompletedTask);
            
            var result = await _controller.CreateBook(createBookDto, book.Id, CancellationToken.None);
            
            var actionResult = Assert.IsType<ActionResult<Book>>(result);
            var createdAtAction = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var returnedBookDto = Assert.IsType<BookDTO>(createdAtAction.Value);
            
            Assert.Equal(bookDTO.Title, returnedBookDto.Title);
            Assert.Equal(bookDTO.ISBN, returnedBookDto.ISBN);
        }
    }
}
