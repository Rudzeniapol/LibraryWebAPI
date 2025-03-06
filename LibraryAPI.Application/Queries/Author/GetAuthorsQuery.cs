using MediatR;

namespace LibraryAPI.Application.Queries.Author;

public class GetAuthorsQuery : IRequest<IEnumerable<Domain.Models.Author>>
{
    
}