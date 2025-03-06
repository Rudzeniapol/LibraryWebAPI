using LibraryAPI.Persistence.Data;
using LibraryAPI.Domain.Models;
using LibraryAPI.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Persistence.Repositories
{
    public class AuthorRepository : BaseRepository<Author>, IAuthorRepository
    {

        public AuthorRepository(LibraryDbContext context) : base(context)
        {
        }
        
        public async Task<IEnumerable<Book>> GetBooksByAuthorAsync(int authorId, CancellationToken cancellationToken = default)
        {
            return await _context.Books
                .Where(b => b.AuthorId == authorId)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> AuthorExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AnyAsync(a => a.Id == id, cancellationToken);
        }
    }
}