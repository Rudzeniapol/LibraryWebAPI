using AutoMapper;
using LibraryAPI.Application.DTOs;
using LibraryAPI.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Application.Queries.Book;

public class GetBooksQueryHandler : IRequestHandler<GetBooksQuery, IEnumerable<Domain.Models.Book>>
{
    private readonly IBookRepository _bookRepository;
    private readonly IMapper _mapper;

    public GetBooksQueryHandler(IBookRepository bookRepository, IMapper mapper)
    {
        _bookRepository = bookRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Domain.Models.Book>> Handle(GetBooksQuery query, CancellationToken cancellationToken = default)
    {
        var booksQuery = await _bookRepository.GetBooksQuery(query.Page,
            query.PageSize, query.Genre, query.Title, cancellationToken);
        return _mapper.Map<IEnumerable<Domain.Models.Book>>(booksQuery);
    }
}