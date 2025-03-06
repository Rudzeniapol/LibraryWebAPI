using MediatR;

namespace LibraryAPI.Application.Queries.User;

public class GetUserByUsernameQuery :IRequest<Domain.Models.User>
{
    public string Username { get; set; }
}