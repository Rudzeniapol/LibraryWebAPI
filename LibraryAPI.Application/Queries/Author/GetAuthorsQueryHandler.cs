using AutoMapper;
using LibraryAPI.Application.DTOs;
using LibraryAPI.Domain.Interfaces;
using MediatR;

namespace LibraryAPI.Application.Queries.Author;

public class GetAuthorsQueryHandler : IRequestHandler<GetAuthorsQuery, IEnumerable<AuthorDTO>>
{
    private readonly IAuthorRepository _authorRepository;
    private readonly IMapper _mapper;
    
    public GetAuthorsQueryHandler(IAuthorRepository authorRepository, IMapper mapper)
    {
        _authorRepository = authorRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AuthorDTO>> Handle(GetAuthorsQuery query, CancellationToken cancellationToken = default)
    {
        return _mapper.Map<IEnumerable<AuthorDTO>>(await _authorRepository.GetAllAsync(cancellationToken));
    }
}