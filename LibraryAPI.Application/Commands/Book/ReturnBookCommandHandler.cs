using LibraryAPI.Application.Exceptions;
using LibraryAPI.Domain.Interfaces;
using MediatR;
using Microsoft.IdentityModel.Tokens;

namespace LibraryAPI.Application.Commands.Book;

public class ReturnBookCommandHandler : IRequestHandler<ReturnBookCommand>
{
    private readonly IBookRepository _bookRepository;
    private readonly IUserRepository _userRepository;

    public ReturnBookCommandHandler(IBookRepository bookRepository, IUserRepository userRepository)
    {
        _bookRepository = bookRepository;
        _userRepository = userRepository;
    }

    public async Task Handle(ReturnBookCommand command, CancellationToken cancellationToken = default)
    {
        var user = _userRepository.GetByIdAsync(command.UserId, cancellationToken).Result;
        if (user.BorrowedBooks.IsNullOrEmpty())
        {
            throw new BadRequestException("У пользователя нет заимствованных книг");
        }
        var book = await _bookRepository.GetByIdAsync(command.BookId, cancellationToken);
        if (book == null)
        {
            throw new NotFoundException($"Книга с id {command.BookId} не найдена");
        }
        if (book.UserId != command.UserId || book.BorrowedAt == null)
        {
            throw new BadRequestException("Невалидная информация о возвращаемой книге");
        }
        book.UserId = null;
        book.BorrowedAt = null;
        book.ReturnBy = null;
        await _bookRepository.ReturnBookAsync(book, cancellationToken);
    }
}