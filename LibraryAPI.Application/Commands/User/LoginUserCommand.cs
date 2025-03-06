using LibraryAPI.Application.DTOs;
using MediatR;

namespace LibraryAPI.Application.Commands.User;

public class LoginUserCommand : IRequest<TokenDto>
{
    public LoginUserDTO LoginUser { get; set; }
}