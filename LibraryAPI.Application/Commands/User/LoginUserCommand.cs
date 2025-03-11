using LibraryAPI.Application.DTOs;
using LibraryAPI.Persistence.DTOs;
using MediatR;

namespace LibraryAPI.Application.Commands.User;

public class LoginUserCommand : IRequest<TokenDTO>
{
    public LoginUserDTO LoginUser { get; set; }
}