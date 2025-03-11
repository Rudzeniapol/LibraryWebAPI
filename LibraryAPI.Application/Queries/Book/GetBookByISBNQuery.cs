using LibraryAPI.Application.DTOs;
using MediatR;

namespace LibraryAPI.Application.Queries.Book;

public class GetBookByISBNQuery : IRequest<BookDTO>
{
    public string ISBN { get; set; }
}