using LibraryAPI.Application.DTOs;
using LibraryAPI.Application.Exceptions;
using LibraryAPI.Application.Services;
using LibraryAPI.Application.Services.Interfaces;
using LibraryAPI.Domain.Interfaces;
using MediatR;

namespace LibraryAPI.Application.Commands.User;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, TokenDTO>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly ITokenService _tokenService;

    public LoginUserCommandHandler(IUserRepository userRepository, IPasswordService passwordService, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _tokenService = tokenService;
    }

    public async Task<TokenDTO> Handle(LoginUserCommand command, CancellationToken cancellationToken = default)
    {
        var existingUser = await _userRepository.GetUserByUsernameAsync(command.LoginUser.Username, cancellationToken);
        if (existingUser == null || !_passwordService.VerifyPassword(existingUser.PasswordHash,command.LoginUser.Password))
            throw new NotFoundException($"Пользователь с именем {command.LoginUser.Username} не найден");

        return await _tokenService.GenerateJwtToken(existingUser, populateExp: true, cancellationToken);
    }
}