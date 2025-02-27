using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using LibraryAPI.DTOs;
using LibraryAPI.Exceptions;
using LibraryAPI.Models;
using LibraryAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace LibraryAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly IImageService _imageService;
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;

        public BooksController(IBookService bookService, IImageService imageService, INotificationService notificationService, IMapper mapper)
        {
            _notificationService = notificationService;
            _imageService = imageService;
            _bookService = bookService;
            _mapper = mapper;
        }
        
        [Authorize(Policy = "AllUsers")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks(
            CancellationToken cancellationToken,
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 10,
            [FromQuery] string? genre = null,
            [FromQuery] string? title = null)
        {
            var booksQuery = _bookService.GetBooksQuery();

            if (!string.IsNullOrEmpty(genre))
                booksQuery = booksQuery.Where(b => b.Genre.Contains(genre));

            if (!string.IsNullOrEmpty(title))
                booksQuery = booksQuery.Where(b => b.Title.Contains(title));

            var books = await booksQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var booksDTO = _mapper.Map<IEnumerable<Book>>(books);
            return Ok(booksDTO);
        }

        [Authorize(Policy = "AllUsers")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id, CancellationToken cancellationToken)
        {
            var book = await _bookService.GetBookByIdAsync(id, cancellationToken);
            var bookDTO = _mapper.Map<Book>(book);
            return Ok(bookDTO);
        }
        
        [Authorize(Policy = "AllUsers")]
        [HttpGet("isbn/{isbn}")]
        public async Task<ActionResult<Book>> GetBookByISBN(string isbn, CancellationToken cancellationToken)
        {
            var book = await _bookService.GetBookByISBNAsync(isbn, cancellationToken);
            if (book == null)
            {
                throw new NotFoundException($"Книга с ISBN \"{isbn}\" не найдена");
            }

            return Ok(book);
        }

        [Authorize(Policy = "AllUsers")]
        [HttpGet("overdue")]
        public async Task<IActionResult> GetOverdueBooks(CancellationToken cancellationToken)
        {
            var overdueBooks = await _notificationService.GetOverdueBooksAsync(cancellationToken);
            return Ok(overdueBooks);
        }

        
        [Authorize(Policy = "AdminOnly")]
        [HttpPost("{id}/upload-image")]
        public async Task<IActionResult> UploadImage(int id, IFormFile file, CancellationToken cancellationToken)
        {
            var imageUrl = await _imageService.UploadImageAsync(file, id, cancellationToken);
            return Ok(new { message = "Изображение загружено", imageUrl });
        }
        
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<ActionResult<Book>> CreateBook(BookDTO bookDto, int id, CancellationToken cancellationToken)
        {
            await _bookService.AddBookAsync(bookDto, cancellationToken);
            return CreatedAtAction(nameof(GetBook), new { id = id }, bookDto);
        }

        [Authorize(Policy = "AllUsers")]
        [HttpPost("{id}/borrow")]
        public async Task<IActionResult> BorrowBook(int id, [FromQuery] int days, CancellationToken cancellationToken)
        {
            var userClaim =  User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (string.IsNullOrEmpty(userClaim) || !int.TryParse(userClaim, out int userId))
            {
                throw new BadRequestException("Ошибка парсинга утверждений пользователя");
            }
            await _bookService.BorrowBookAsync(id, userId, days, cancellationToken);
            return Ok("Книга успешно взята.");
        }

        [Authorize(Policy = "AllUsers")]
        [HttpPost("{id}/return")]
        public async Task<IActionResult> ReturnBook(int id, CancellationToken cancellationToken)
        {
            var userReturn = User.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(userReturn) || !int.TryParse(userReturn, out int userId))
            {
                throw new BadRequestException("У пользователя нет заимствованных книг");
            }
            await _bookService.ReturnBookAsync(id, userId, cancellationToken);
            return Ok("Книга успешно возвращена.");
        }
        
        [Authorize(Policy = "AdminOnly")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, BookDTO book, CancellationToken cancellationToken)
        {
            await _bookService.UpdateBookAsync(book, id, cancellationToken);
            return NoContent();
        }
        
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id, CancellationToken cancellationToken)
        {
            await _bookService.DeleteBookAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
