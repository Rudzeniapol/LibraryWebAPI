using LibraryAPI.Application.DTOs;
using MediatR;

namespace LibraryAPI.Application.Commands.User;

public class RegisterUserCommand : IRequest<Domain.Models.User>
{
    public RegisterUserDTO RegisterUser { get; set; }
}