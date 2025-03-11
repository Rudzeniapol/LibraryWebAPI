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

        public async Task<Author?> AuthorExistsAsync(Author author, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FirstOrDefaultAsync(a => a.FirstName.ToLower() == author.FirstName.ToLower() && a.LastName.ToLower() == author.LastName.ToLower() && a.Country.ToLower() == author.Country.ToLower() && a.DateOfBirth.Date == author.DateOfBirth.Date, cancellationToken);
        }
    }
}