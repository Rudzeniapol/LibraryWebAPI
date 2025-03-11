using AutoMapper;
using LibraryAPI.Application.DTOs;
using LibraryAPI.Application.Exceptions;
using LibraryAPI.Domain.Interfaces;
using MediatR;

namespace LibraryAPI.Application.Queries.Book;

public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, BookDTO>
{
    private readonly IBookRepository _bookRepository;
    private readonly IMapper _mapper;
    
    public GetBookByIdQueryHandler(IBookRepository bookRepository, IMapper mapper)
    {
        _bookRepository = bookRepository;
        _mapper = mapper;
    }

    public async Task<BookDTO> Handle(GetBookByIdQuery query, CancellationToken cancellationToken = default)
    {
        var book = await _bookRepository.GetByIdAsync(query.Id, cancellationToken);
        if (book == null)
        {
            throw new NotFoundException($"Книга с id {query.Id} не найдена");
        }
        return _mapper.Map<BookDTO>(book);
    }
}