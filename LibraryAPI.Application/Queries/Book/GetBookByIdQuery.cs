using LibraryAPI.Application.DTOs;
using MediatR;

namespace LibraryAPI.Application.Queries.Book;

public class GetBookByIdQuery :IRequest<BookDTO>
{
    public int Id { get; set; }
}