using LibraryAPI.Domain.Interfaces;
using MediatR;

namespace LibraryAPI.Application.Queries.Author;

public class GetAuthorsQueryHandler : IRequestHandler<GetAuthorsQuery, IEnumerable<Domain.Models.Author>>
{
    private readonly IAuthorRepository _authorRepository;
    
    public GetAuthorsQueryHandler(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public async Task<IEnumerable<LibraryAPI.Domain.Models.Author>> Handle(GetAuthorsQuery query, CancellationToken cancellationToken = default)
    {
        return await _authorRepository.GetAllAsync(cancellationToken);
    }
}