using LibraryAPI.Application.DTOs;
using MediatR;

namespace LibraryAPI.Application.Queries.Author;

public class GetBooksByAuthorQuery :IRequest<IEnumerable<BookDTO>>
{
    public int AuthorId { get; set; }
}