using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using LibraryAPI.DTOs;
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
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks(
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
                .ToListAsync();

            var booksDTO = _mapper.Map<IEnumerable<Book>>(books);
            return Ok(booksDTO);
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null) return NotFound();

            var bookDTO = _mapper.Map<Book>(book);
            return Ok(bookDTO);
        }
        
        [HttpGet("isbn/{isbn}")]
        public async Task<ActionResult<Book>> GetBookByISBN(string isbn)
        {
            var book = await _bookService.GetBookByISBNAsync(isbn);
            return book != null ? Ok(book) : NotFound();
        }

        [HttpGet("overdue")]
        public async Task<IActionResult> GetOverdueBooks()
        {
            var overdueBooks = await _notificationService.GetOverdueBooksAsync();
            return Ok(overdueBooks);
        }

        
        [Authorize(Roles = "admin")]
        [HttpPost("{id}/upload-image")]
        public async Task<IActionResult> UploadImage(int id, IFormFile file)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
                return NotFound("Книга не найдена");

            var imageUrl = await _imageService.UploadImageAsync(file, book.Title);
            if (imageUrl == null)
                return BadRequest("Ошибка загрузки изображения");

            book.ImageUrl = imageUrl;
            await _bookService.UpdateBookAsync(book);

            return Ok(new { message = "Изображение загружено", imageUrl });
        }
        
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult<Book>> CreateBook(CreateBookDTO createBookDto)
        {
            var book = _mapper.Map<Book>(createBookDto);
            await _bookService.AddBookAsync(book);

            var bookDTO = _mapper.Map<Book>(book);
            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, bookDTO);
        }


        [HttpPost("{id}/borrow")]
        public async Task<IActionResult> BorrowBook(int id, [FromQuery] int days)
        {
            var userClaim =  User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (string.IsNullOrEmpty(userClaim) || !int.TryParse(userClaim, out int userId))
            {
                return Unauthorized("Ошибка аутентификации: не удалось определить пользователя.");
            }
            
            return Ok("Книга успешно взята.");
        }

        
        [HttpPost("{id}/return")]
        public async Task<IActionResult> ReturnBook(int id)
        {
            var userReturn = User.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(userReturn) || !int.TryParse(userReturn, out int userId))
            {
                return Unauthorized("Ошибка аутентификации: не удалось определить пользователя.");
            }
            await _bookService.ReturnBookAsync(id, userId);

            return Ok("Книга успешно возвращена.");
        }
        
        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, Book book)
        {
            if (id != book.Id) return BadRequest("ID книги не совпадает.");
            await _bookService.UpdateBookAsync(book);
            return NoContent();
        }
        
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            await _bookService.DeleteBookAsync(id);
            return NoContent();
        }
    }
}
