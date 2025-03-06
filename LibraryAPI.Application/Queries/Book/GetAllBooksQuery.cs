using MediatR;

namespace LibraryAPI.Application.Queries.Book;

public class GetAllBooksQuery : IRequest<IEnumerable<Domain.Models.Book>>
{
    
}