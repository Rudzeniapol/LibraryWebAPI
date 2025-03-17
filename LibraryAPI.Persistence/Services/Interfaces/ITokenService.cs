using System.Security.Claims;
using LibraryAPI.Domain.Models;
using LibraryAPI.Persistence.DTOs;

namespace LibraryAPI.Persistence.Services.Interfaces;

public interface ITokenService
{
    Task<TokenDTO> GenerateJwtToken(User user, bool populateExp, CancellationToken cancellationToken = default);
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token, CancellationToken cancellationToken = default);
}