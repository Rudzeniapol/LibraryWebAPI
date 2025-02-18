using LibraryAPI.Models;

namespace LibraryAPI.Services.Interfaces
{
    public interface IUserService
    {
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<User> RegisterUserAsync(string username, string password, string role);
    }
}