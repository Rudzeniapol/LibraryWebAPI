using System.Security.Claims;
using LibraryAPI.Application.DTOs;
using LibraryAPI.Domain.Models;

namespace LibraryAPI.Application.Services.Interfaces;

public interface ITokenService
{
    Task<TokenDTO> GenerateJwtToken(User user, bool populateExp, CancellationToken cancellationToken = default);
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token, CancellationToken cancellationToken = default);
}