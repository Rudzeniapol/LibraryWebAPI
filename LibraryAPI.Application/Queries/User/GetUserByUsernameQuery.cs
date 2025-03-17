using LibraryAPI.Application.DTOs;
using MediatR;

namespace LibraryAPI.Application.Queries.User;

public class GetUserByUsernameQuery :IRequest<UserDTO>
{
    public string Username { get; set; }
}