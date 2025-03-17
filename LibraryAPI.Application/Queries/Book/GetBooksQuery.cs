using LibraryAPI.Application.DTOs;
using MediatR;

namespace LibraryAPI.Application.Queries.Book;

public class GetBooksQuery :IRequest<IEnumerable<BookDTO>>
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public string? Genre { get; set; }
    public string? Title { get; set; }
}