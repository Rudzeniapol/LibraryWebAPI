using LibraryAPI.Application.DTOs;
using MediatR;

namespace LibraryAPI.Application.Queries.Author;

public class GetAuthorByIdQuery : IRequest<AuthorDTO>
{
    public int Id { get; set; }
}