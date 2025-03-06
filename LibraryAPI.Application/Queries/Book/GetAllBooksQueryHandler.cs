using LibraryAPI.Domain.Interfaces;
using MediatR;

namespace LibraryAPI.Application.Queries.Book;

public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, IEnumerable<Domain.Models.Book>>
{
    private readonly IBookRepository _bookRepository;

    public GetAllBooksQueryHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<IEnumerable<Domain.Models.Book>> Handle(GetAllBooksQuery query, CancellationToken cancellationToken = default)
    {
        return await _bookRepository.GetAllAsync(cancellationToken);
    }
}