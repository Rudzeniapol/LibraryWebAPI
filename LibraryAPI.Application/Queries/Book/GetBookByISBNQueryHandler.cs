using LibraryAPI.Application.Exceptions;
using LibraryAPI.Domain.Interfaces;
using MediatR;

namespace LibraryAPI.Application.Queries.Book;

public class GetBookByISBNQueryHandler : IRequestHandler<GetBookByISBNQuery, Domain.Models.Book>
{
    private readonly IBookRepository _bookRepository;

    public GetBookByISBNQueryHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<Domain.Models.Book> Handle(GetBookByISBNQuery query, CancellationToken cancellationToken = default)
    {
        var book = await _bookRepository.GetBookByISBNAsync(query.ISBN, cancellationToken);
        if (book == null)
        {
            throw new NotFoundException($"книга с ISBN {query.ISBN} не найдена");
        }
        return book;
    }
}