using LibraryAPI.Models;
using LibraryAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorsController(IAuthorService authorService)
        {
            _authorService = authorService;
        }
        
        [Authorize(Policy = "AllUsers")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthors(CancellationToken cancellationToken)
        {
            var authors = await _authorService.GetAllAuthorsAsync(cancellationToken);
            return Ok(authors);
        }
        
        [Authorize(Policy = "AllUsers")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Author>> GetAuthor(int id, CancellationToken cancellationToken)
        {
            var author = await _authorService.GetAuthorByIdAsync(id, cancellationToken);
            return author != null ? Ok(author) : NotFound();
        }
        
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<ActionResult<Author>> CreateAuthor(Author author, CancellationToken cancellationToken)
        {
            await _authorService.AddAuthorAsync(author, cancellationToken);
            return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, author);
        }
        
        [Authorize(Policy = "AdminOnly")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, Author author, CancellationToken cancellationToken)
        {
            if (id != author.Id) return BadRequest("ID автора не совпадает.");
            await _authorService.UpdateAuthorAsync(author, cancellationToken);
            return NoContent();
        }
        
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id, CancellationToken cancellationToken)
        {
            await _authorService.DeleteAuthorAsync(id, cancellationToken);
            return NoContent();
        }
        
        [Authorize(Policy = "AllUsers")]
        [HttpGet("{id}/books")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooksByAuthor(int id)
        {
            var books = await _authorService.GetBooksByAuthorAsync(id);
            return Ok(books);
        }
    }
}
