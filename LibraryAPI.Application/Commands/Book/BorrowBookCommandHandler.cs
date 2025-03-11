using LibraryAPI.Application.Exceptions;
using LibraryAPI.Domain.Interfaces;
using MediatR;

namespace LibraryAPI.Application.Commands.Book;

public class BorrowBookCommandHandler : IRequestHandler<BorrowBookCommand>
{
    private readonly IBookRepository _bookRepository;
    private readonly IUserRepository _userRepository;

    public BorrowBookCommandHandler(IBookRepository bookRepository, IUserRepository userRepository)
    {
        _bookRepository = bookRepository;
        _userRepository = userRepository;
    }

    public async Task Handle(BorrowBookCommand command, CancellationToken cancellationToken = default)
    {
        var book = await _bookRepository.GetByIdAsync(command.bookId, cancellationToken);
        if (book == null)
        {
            throw new NotFoundException($"Книга с id {command.bookId} не найдена");
        }

        if (book.BorrowedAt != null)
        {
            throw new BadRequestException("Книга занята");
        }
        
        var user = await _userRepository.GetByIdAsync(command.userId, cancellationToken);
        if (user == null)
        {
            throw new NotFoundException($"Пользователь с id {command.userId} не найден");
        }
        book.UserId = command.userId;
        book.BorrowedAt = DateTime.UtcNow;
        book.ReturnBy = DateTime.UtcNow.AddDays(command.days);
        await _bookRepository.BorrowBookAsync(book, cancellationToken);
    }
}