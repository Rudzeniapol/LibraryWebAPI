using LibraryAPI.Application.DTOs;
using MediatR;

namespace LibraryAPI.Application.Queries.Book;

public class GetAllBooksQuery : IRequest<IEnumerable<BookDTO>>
{
    
}