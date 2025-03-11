using MediatR;

namespace LibraryAPI.Application.Commands.Book;

public class BorrowBookCommand : IRequest
{
    public int bookId { get; set; }
    public int userId { get; set; }
    public int days { get; set; }
}