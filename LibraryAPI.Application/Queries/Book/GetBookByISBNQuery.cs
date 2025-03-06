using MediatR;

namespace LibraryAPI.Application.Queries.Book;

public class GetBookByISBNQuery : IRequest<Domain.Models.Book>
{
    public string ISBN { get; set; }
}