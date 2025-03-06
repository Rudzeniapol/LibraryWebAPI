using LibraryAPI.Persistence.Data;
using LibraryAPI.Domain.Models;
using LibraryAPI.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Persistence.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(LibraryDbContext context) : base(context)
    {
    }
    
    public async Task<User?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Username.Equals(username), cancellationToken);
    }
}