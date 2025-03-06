using MediatR;

namespace LibraryAPI.Application.Queries.Book;

public class GetBooksQuery :IRequest<IEnumerable<Domain.Models.Book>>
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public string? Genre { get; set; }
    public string? Title { get; set; }
}