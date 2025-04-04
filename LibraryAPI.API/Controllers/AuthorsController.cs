using LibraryAPI.Application.Commands.Author;
using LibraryAPI.Application.DTOs;
using LibraryAPI.Application.Exceptions;
using LibraryAPI.Application.Queries.Author;
using LibraryAPI.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthorsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [Authorize(Policy = "AllUsers")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDTO>>> GetAuthors(CancellationToken cancellationToken)
        {
            GetAuthorsQuery query = new GetAuthorsQuery();
            var authors = await _mediator.Send(query, cancellationToken);
            return Ok(authors);
        }
        
        [Authorize(Policy = "AllUsers")]
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDTO>> GetAuthor(int id, CancellationToken cancellationToken)
        {
            GetAuthorByIdQuery query = new GetAuthorByIdQuery()
            {
                Id = id
            };
            var author = await _mediator.Send(query, cancellationToken);
            return Ok(author);
        }
        
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<ActionResult<AuthorDTO>> CreateAuthor([FromBody] AddAuthorCommand command, CancellationToken cancellationToken)
        {
            var newAuthorId = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetAuthor), new { id = newAuthorId }, command.Author);
        }
        
        [Authorize(Policy = "AdminOnly")]   
        [HttpPut]
        public async Task<IActionResult> UpdateAuthor([FromBody] UpdateAuthorCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
        
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id, CancellationToken cancellationToken)
        {
            DeleteAuthorCommand command = new DeleteAuthorCommand()
            {
                Id = id
            };
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
        
        [Authorize(Policy = "AllUsers")]
        [HttpGet("{id}/books")]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetBooksByAuthor(int id, CancellationToken cancellationToken)
        {
            GetBooksByAuthorQuery query = new GetBooksByAuthorQuery()
            {
                AuthorId = id
            };
            var books = await _mediator.Send(query, cancellationToken);
            return Ok(books);
        }
    }
}
