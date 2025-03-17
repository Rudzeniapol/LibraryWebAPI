using LibraryAPI.Application.DTOs;
using LibraryAPI.Persistence.DTOs;
using MediatR;

namespace LibraryAPI.Application.Commands.Token;

public class RefreshTokenCommand : IRequest<TokenDTO>
{
    public TokenDTO Token { get; set; }
}