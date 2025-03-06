using MediatR;

namespace LibraryAPI.Application.Commands.Book;

public class BorrowBookCommand : IRequest
{
    public int bookId;
    public int userId;
    public int days;
}