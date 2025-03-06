using MediatR;

namespace LibraryAPI.Application.Queries.Author;

public class GetBooksByAuthorQuery :IRequest<IEnumerable<Domain.Models.Book>>
{
    public int AuthorId { get; set; }
}