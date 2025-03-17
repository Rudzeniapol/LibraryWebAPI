using AutoMapper;
using LibraryAPI.Application.DTOs;
using LibraryAPI.Application.Exceptions;
using LibraryAPI.Domain.Interfaces;
using MediatR;

namespace LibraryAPI.Application.Queries.Author;

public class GetAuthorByIdQueryHandler : IRequestHandler<GetAuthorByIdQuery, AuthorDTO>
{
    private readonly IAuthorRepository _authorRepository;
    private readonly IMapper _mapper;
    
    public GetAuthorByIdQueryHandler(IAuthorRepository authorRepository, IMapper mapper)
    {
        _authorRepository = authorRepository;
        _mapper = mapper;
    }

    public async Task<AuthorDTO> Handle(GetAuthorByIdQuery query, CancellationToken cancellationToken = default)
    {
        var author = await _authorRepository.GetByIdAsync(query.Id, cancellationToken);
        if (author == null)
        {
            throw new NotFoundException($"Автор с id {query.Id} не найден");
        }
        return _mapper.Map<AuthorDTO>(author);
    }
}