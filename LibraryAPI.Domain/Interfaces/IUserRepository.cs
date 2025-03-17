using LibraryAPI.Domain.Models;

namespace LibraryAPI.Domain.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken = default);
}