using LibraryAPI.Domain.Interfaces;
using LibraryAPI.Persistence.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Application.Queries.Notification;

public class GetOverdueBooksQueryHandler : IRequestHandler<GetOverdueBooksQuery, List<string>>
{
    private readonly IBookRepository _bookRepository;

    public GetOverdueBooksQueryHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<List<string>> Handle(GetOverdueBooksQuery query, CancellationToken cancellationToken)
    {
        return await _bookRepository.GetOverdueBooks(cancellationToken);
    }
}