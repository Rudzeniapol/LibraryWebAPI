using LibraryAPI.API.Controllers;
using LibraryAPI.Application.Commands.Book;
using LibraryAPI.Application.DTOs;
using LibraryAPI.Application.Queries.Book;
using LibraryAPI.Domain.Models;
using LibraryAPI.Persistence.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryAPI.Tests
{
    public class BooksControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock = new();
        private readonly Mock<IImageService> _imageServiceMock = new();
        private readonly BooksController _controller;

        public BooksControllerTests()
        {
            _controller = new BooksController(_imageServiceMock.Object, _mediatorMock.Object);
        }

        [Fact]
        public async Task GetBook_ReturnsOk_WhenBookExists()
        {
            var book = new BookDTO { Title = "1984" };
            _mediatorMock.Setup(m => m.Send(It.Is<GetBookByIdQuery>(q => q.Id == 1), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(book);

            var result = await _controller.GetBook(1, CancellationToken.None);
            var actionResult = Assert.IsType<ActionResult<Book>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Equal(book, okResult.Value);
        }

        [Fact]
        public async Task CreateBook_ReturnsCreatedAtAction_WithValidData()
        {
            var command = new AddBookCommand { Book = new BookDTO() };
            _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                         .Returns(Task.FromResult(1));

            var result = await _controller.CreateBook(command, CancellationToken.None);
            var createdAtResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(nameof(BooksController.GetBook), createdAtResult.ActionName);
        }

        [Fact]
        public async Task UpdateBook_ReturnsNoContent_OnSuccess()
        {
            var bookDto = new BookDTO();
            var command = new UpdateBookCommand { Id = 1, Book = bookDto };
            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateBookCommand>(), It.IsAny<CancellationToken>()))
                         .Returns(Task.FromResult(Unit.Value));

            var result = await _controller.UpdateBook(1, bookDto, CancellationToken.None);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteBook_ReturnsNoContent_WhenCommandValid()
        {
            var command = new DeleteBookCommand { bookId = 1 };
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteBookCommand>(), It.IsAny<CancellationToken>()))
                         .Returns(Task.FromResult(Unit.Value));

            var result = await _controller.DeleteBook(1, CancellationToken.None);
            Assert.IsType<NoContentResult>(result);
        }

    }
}
