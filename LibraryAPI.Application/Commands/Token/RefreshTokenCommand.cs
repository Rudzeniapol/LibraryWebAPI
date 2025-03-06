using LibraryAPI.Application.DTOs;
using MediatR;

namespace LibraryAPI.Application.Commands.Token;

public class RefreshTokenCommand : IRequest<TokenDto>
{
    public TokenDto Token { get; set; }
}