using MediatR;

namespace LibraryAPI.Application.Queries.User;

public class GetUserByIdQuery : IRequest<Domain.Models.User>
{
    public int Id { get; set; }
}