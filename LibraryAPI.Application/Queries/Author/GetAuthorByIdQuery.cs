using MediatR;

namespace LibraryAPI.Application.Queries.Author;

public class GetAuthorByIdQuery : IRequest<Domain.Models.Author>
{
    public int Id { get; set; }
}