using LibraryAPI.Models;

namespace LibraryAPI.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserByUsernameAsync(string username);
    Task AddUserAsync(User user);
    Task<User?> GetUserByIdAsync(int userId);
    
}