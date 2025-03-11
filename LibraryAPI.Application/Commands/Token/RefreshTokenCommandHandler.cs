using LibraryAPI.Application.DTOs;
using LibraryAPI.Application.Exceptions;
using LibraryAPI.Persistence.Services.Interfaces;
using LibraryAPI.Domain.Interfaces;
using LibraryAPI.Persistence.DTOs;
using LibraryAPI.Persistence.Repositories;
using MediatR;

namespace LibraryAPI.Application.Commands.Token;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, TokenDTO>
{
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userRepository;

    public RefreshTokenCommandHandler(ITokenService tokenService, IUserRepository userRepository)
    {
        _tokenService = tokenService;
        _userRepository = userRepository;
    }
    
    public async Task<TokenDTO> Handle(RefreshTokenCommand tokenDto, CancellationToken cancellationToken = default)
    {
        var principal = _tokenService.GetPrincipalFromExpiredToken(tokenDto.Token.AccessToken);
        var user = await _userRepository.GetUserByUsernameAsync(principal.Identity.Name, cancellationToken);
        if (user == null || user.RefreshToken != tokenDto.Token.RefreshToken ||
            user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            throw new BadRequestException("Невалидный токен либо истёк срок действия токена");
        }

        return await _tokenService.GenerateJwtToken(user, populateExp: false, cancellationToken);
    }
}