using AutoMapper;
using LibraryAPI.Application.DTOs;
using LibraryAPI.Application.Exceptions;
using LibraryAPI.Domain.Interfaces;
using LibraryAPI.Domain.Models;
using MediatR;

namespace LibraryAPI.Application.Queries.Author;

public class GetBooksByAuthorQueryHandler : IRequestHandler<GetBooksByAuthorQuery, IEnumerable<BookDTO>>
{
    private readonly IAuthorRepository _authorRepository;
    private readonly IMapper _mapper;

    public GetBooksByAuthorQueryHandler(IAuthorRepository authorRepository, IMapper mapper)
    {
        _authorRepository = authorRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<BookDTO>> Handle(GetBooksByAuthorQuery query, CancellationToken cancellationToken)
    {
        var author = await _authorRepository.GetByIdAsync(query.AuthorId, cancellationToken);
        if (author == null)
        {
            throw new NotFoundException($"Автор с id {query.AuthorId} не найден");
        }

        var books = await _authorRepository.GetBooksByAuthorAsync(query.AuthorId, cancellationToken);
        return _mapper.Map<IEnumerable<BookDTO>>(books);;
    }
}