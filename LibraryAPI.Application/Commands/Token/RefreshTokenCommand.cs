using LibraryAPI.Application.DTOs;
using MediatR;

namespace LibraryAPI.Application.Commands.Token;

public class RefreshTokenCommand : IRequest<TokenDTO>
{
    public TokenDTO Token { get; set; }
}