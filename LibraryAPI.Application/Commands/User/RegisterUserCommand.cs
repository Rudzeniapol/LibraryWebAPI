using LibraryAPI.Application.DTOs;
using MediatR;

namespace LibraryAPI.Application.Commands.User;

public class RegisterUserCommand : IRequest<RegisterUserDTO>
{
    public RegisterUserDTO RegisterUser { get; set; }
}