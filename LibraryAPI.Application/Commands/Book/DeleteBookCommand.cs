using MediatR;

namespace LibraryAPI.Application.Commands.Book;

public class DeleteBookCommand : IRequest
{
    public int bookId;
}