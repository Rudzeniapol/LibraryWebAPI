using LibraryAPI.Application.DTOs;
using LibraryAPI.Domain.Models;

namespace LibraryAPI.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<User?> GetUserByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<User?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken = default);
        Task<User?> RegisterUserAsync(RegisterUserDTO registerUser, CancellationToken cancellationToken = default);
        Task<TokenDto?> LoginUserAsync(LoginUserDTO user, CancellationToken cancellationToken = default);
        Task<TokenDto> RefreshToken(TokenDto tokenDto, CancellationToken cancellationToken = default);
    }
}