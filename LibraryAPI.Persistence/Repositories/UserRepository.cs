using LibraryAPI.Persistence.Data;
using LibraryAPI.Domain.Models;
using LibraryAPI.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly LibraryDbContext _context;

    public UserRepository(LibraryDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username.Equals(username), cancellationToken);
    }

    public async Task AddUserAsync(User user, CancellationToken cancellationToken = default)
    {
        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateUserAsync(User user, CancellationToken cancellationToken = default)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<User?> GetUserByIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
    }
}