using MediatR;

namespace LibraryAPI.Application.Commands.Book;

public class ReturnBookCommand : IRequest
{
    public int BookId { get; set; }
    public int UserId { get; set; }
}