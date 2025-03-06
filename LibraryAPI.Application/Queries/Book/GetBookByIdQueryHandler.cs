using LibraryAPI.Application.Exceptions;
using LibraryAPI.Domain.Interfaces;
using MediatR;

namespace LibraryAPI.Application.Queries.Book;

public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, Domain.Models.Book>
{
    private readonly IBookRepository _bookRepository;

    public GetBookByIdQueryHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<Domain.Models.Book> Handle(GetBookByIdQuery query, CancellationToken cancellationToken = default)
    {
        var book = await _bookRepository.GetByIdAsync(query.Id, cancellationToken);
        if (book == null)
        {
            throw new NotFoundException($"Книга с id {query.Id} не найдена");
        }
        return book;
    }
}