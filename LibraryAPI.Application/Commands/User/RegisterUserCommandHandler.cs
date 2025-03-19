using AutoMapper;
using LibraryAPI.Application.DTOs;
using LibraryAPI.Application.Exceptions;
using LibraryAPI.Application.Services.Interfaces;
using LibraryAPI.Domain.Interfaces;
using MediatR;

namespace LibraryAPI.Application.Commands.User;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegisterUserDTO>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly IMapper _mapper;
    
    
    public RegisterUserCommandHandler(IUserRepository userRepository, IPasswordService passwordService, IMapper mapper)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _mapper = mapper;
    }

    public async Task<RegisterUserDTO> Handle(RegisterUserCommand command, CancellationToken cancellationToken = default)
    {
        var existingUser = await _userRepository.GetUserByUsernameAsync(command.RegisterUser.Username, cancellationToken);
        if (existingUser != null)
            throw new EntityExistsException($"Пользователь с именем {command.RegisterUser.Username} уже существует");
        var hashedPassword = _passwordService.HashPassword(command.RegisterUser.Password);
        var user = new Domain.Models.User
        {
            Username = command.RegisterUser.Username,
            PasswordHash = hashedPassword,
            Role = command.RegisterUser.Role,
        };
        await _userRepository.AddAsync(user, cancellationToken);
        return _mapper.Map<RegisterUserDTO>(user);
    }
}