using LibraryAPI.Application.DTOs;
using MediatR;

namespace LibraryAPI.Application.Queries.Author;

public class GetAuthorsQuery : IRequest<IEnumerable<AuthorDTO>>
{
    
}