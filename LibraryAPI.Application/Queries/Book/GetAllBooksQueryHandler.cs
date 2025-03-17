using AutoMapper;
using LibraryAPI.Application.DTOs;
using LibraryAPI.Domain.Interfaces;
using MediatR;

namespace LibraryAPI.Application.Queries.Book;

public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, IEnumerable<BookDTO>>
{
    private readonly IBookRepository _bookRepository;
    private readonly IMapper _mapper;
    
    public GetAllBooksQueryHandler(IBookRepository bookRepository, IMapper mapper)
    {
        _bookRepository = bookRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<BookDTO>> Handle(GetAllBooksQuery query, CancellationToken cancellationToken = default)
    {
        var books = await _bookRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<BookDTO>>(books);
    }
}