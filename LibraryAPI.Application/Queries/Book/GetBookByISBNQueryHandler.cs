using AutoMapper;
using LibraryAPI.Application.DTOs;
using LibraryAPI.Application.Exceptions;
using LibraryAPI.Domain.Interfaces;
using MediatR;

namespace LibraryAPI.Application.Queries.Book;

public class GetBookByISBNQueryHandler : IRequestHandler<GetBookByISBNQuery, BookDTO>
{
    private readonly IBookRepository _bookRepository;
    private readonly IMapper _mapper;

    public GetBookByISBNQueryHandler(IBookRepository bookRepository, IMapper mapper)
    {
        _bookRepository = bookRepository;
        _mapper = mapper;
    }

    public async Task<BookDTO> Handle(GetBookByISBNQuery query, CancellationToken cancellationToken = default)
    {
        var book = await _bookRepository.GetBookByISBNAsync(query.ISBN, cancellationToken);
        if (book == null)
        {
            throw new NotFoundException($"книга с ISBN {query.ISBN} не найдена");
        }
        return _mapper.Map<BookDTO>(book);
    }
}