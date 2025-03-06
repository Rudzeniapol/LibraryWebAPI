using LibraryAPI.Application.Exceptions;
using LibraryAPI.Domain.Interfaces;
using LibraryAPI.Domain.Models;
using MediatR;

namespace LibraryAPI.Application.Queries.Author;

public class GetBooksByAuthorQueryHandler : IRequestHandler<GetBooksByAuthorQuery, IEnumerable<Domain.Models.Book>>
{
    private readonly IAuthorRepository _authorRepository;

    public GetBooksByAuthorQueryHandler(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public async Task<IEnumerable<Domain.Models.Book>> Handle(GetBooksByAuthorQuery query, CancellationToken cancellationToken)
    {
        var author = await _authorRepository.GetByIdAsync(query.AuthorId, cancellationToken);
        if (author == null)
        {
            throw new NotFoundException($"Автор с id {query.AuthorId} не найден");
        }
        return await _authorRepository.GetBooksByAuthorAsync(query.AuthorId, cancellationToken);
    }
}