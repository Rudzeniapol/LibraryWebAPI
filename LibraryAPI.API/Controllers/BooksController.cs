using System.IdentityModel.Tokens.Jwt;
using LibraryAPI.Application.Commands.Author;
using LibraryAPI.Application.Commands.Book;
using LibraryAPI.Application.DTOs;
using LibraryAPI.Application.Exceptions;
using LibraryAPI.Application.Queries.Book;
using LibraryAPI.Application.Queries.Notification;
using LibraryAPI.Domain.Models;
using LibraryAPI.Persistence.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace LibraryAPI.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IImageService _imageService;
        private readonly IMediator _mediator;

        public BooksController(IImageService imageService, IMediator mediator)
        {
            _imageService = imageService;
            _mediator = mediator;
        }
        
        [Authorize(Policy = "AllUsers")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> QGetBooks(CancellationToken cancellationToken,
            [FromQuery] GetBooksQuery query)
        {
            var booksQuery = await _mediator.Send(query, cancellationToken);
            return Ok(booksQuery);
        }

        [Authorize(Policy = "AllUsers")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id, CancellationToken cancellationToken)
        {
            GetBookByIdQuery query = new GetBookByIdQuery()
            {
                Id = id
            };
            var book = await _mediator.Send(query, cancellationToken);
            return Ok(book);
        }
        
        [Authorize(Policy = "AllUsers")]
        [HttpGet("isbn/{isbn}")]
        public async Task<ActionResult<Book>> GetBookByISBN(string isbn, CancellationToken cancellationToken)
        {
            GetBookByISBNQuery query = new GetBookByISBNQuery()
            {
                ISBN = isbn
            };
            var book = await _mediator.Send(query, cancellationToken);
            return Ok(book);
        }

        [Authorize(Policy = "AllUsers")]
        [HttpGet("overdue")]
        public async Task<IActionResult> GetOverdueBooks(CancellationToken cancellationToken)
        {
            GetOverdueBooksQuery query = new GetOverdueBooksQuery();
            var overdueBooks = await _mediator.Send(query, cancellationToken);
            return Ok(overdueBooks);
        }

        
        [Authorize(Policy = "AdminOnly")]
        [HttpPost("{id}/upload-image")]
        public async Task<IActionResult> UploadImage(int id, IFormFile file, CancellationToken cancellationToken)
        {
            UploadBookImageCommand command = new UploadBookImageCommand()
            {
                BookId = id,
                File = file
            };
            var imageUrl = await _mediator.Send(command, cancellationToken);
            return Ok(new { message = "Изображение загружено", imageUrl });
        }
        
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<ActionResult<Book>> CreateBook([FromBody] AddBookCommand command, CancellationToken cancellationToken)
        {
            var bookId = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetBook), new{ id = bookId },  command);
        }

        [Authorize(Policy = "AllUsers")]
        [HttpPost("borrow")]
        public async Task<IActionResult> BorrowBook([FromBody] BorrowBookCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);
            return Ok("Книга успешно взята.");
        }

        [Authorize(Policy = "AllUsers")]
        [HttpPost("return")]
        public async Task<IActionResult> ReturnBook([FromBody] ReturnBookCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);
            return Ok("Книга успешно возвращена.");
        }
        
        [Authorize(Policy = "AdminOnly")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id,[FromBody] BookDTO book, CancellationToken cancellationToken)
        {
            UpdateBookCommand command = new UpdateBookCommand()
            {
                Id = id,
                Book = book
            };
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
        
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id, CancellationToken cancellationToken)
        {
            DeleteAuthorCommand command = new DeleteAuthorCommand()
            {
                Id = id
            };
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
    }
}
