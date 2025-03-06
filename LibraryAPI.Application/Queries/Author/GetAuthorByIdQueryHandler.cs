using LibraryAPI.Application.Exceptions;
using LibraryAPI.Domain.Interfaces;
using MediatR;

namespace LibraryAPI.Application.Queries.Author;

public class GetAuthorByIdQueryHandler : IRequestHandler<GetAuthorByIdQuery, Domain.Models.Author>
{
    private readonly IAuthorRepository _authorRepository;
    
    public GetAuthorByIdQueryHandler(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public async Task<Domain.Models.Author> Handle(GetAuthorByIdQuery query, CancellationToken cancellationToken = default)
    {
        var author = await _authorRepository.GetByIdAsync(query.Id, cancellationToken);
        if (author == null)
        {
            throw new NotFoundException($"Автор с id {query.Id} не найден");
        }
        return author;
    }
}