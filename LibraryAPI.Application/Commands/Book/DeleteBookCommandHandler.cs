using LibraryAPI.Application.Exceptions;
using LibraryAPI.Domain.Interfaces;
using MediatR;

namespace LibraryAPI.Application.Commands.Book;

public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand>
{
    private readonly IBookRepository _bookRepository;

    public DeleteBookCommandHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task Handle(DeleteBookCommand command, CancellationToken cancellationToken = default)
    {
        var book = await _bookRepository.GetByIdAsync(command.bookId, cancellationToken);
        if (book == null)
        {
            throw new NotFoundException($"Книга с id {command.bookId} не найдена");
        }
        await _bookRepository.DeleteAsync(book, cancellationToken);
    }
}