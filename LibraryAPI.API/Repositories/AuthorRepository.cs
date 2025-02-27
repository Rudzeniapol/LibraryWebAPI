using LibraryAPI.API.Data;
using LibraryAPI.Domain.Models;
using LibraryAPI.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.API.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly LibraryDbContext _context;

        public AuthorRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Author>> GetAllAuthorsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Authors.Include(a => a.Books).ToListAsync(cancellationToken);
        }

        public async Task<Author?> GetAuthorByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Authors.Include(a => a.Books)
                .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        }

        public async Task AddAuthorAsync(Author author, CancellationToken cancellationToken = default)
        {
            _context.Authors.Add(author);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAuthorAsync(Author author, CancellationToken cancellationToken = default)
        {
            _context.Authors.Update(author);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAuthorAsync(int id, CancellationToken cancellationToken = default)
        {
            var author = await _context.Authors.FindAsync(id, cancellationToken);
            if (author != null)
            {
                _context.Authors.Remove(author);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<IEnumerable<Book>> GetBooksByAuthorAsync(int authorId, CancellationToken cancellationToken = default)
        {
            return await _context.Books.Where(b => b.AuthorId == authorId).ToListAsync(cancellationToken);
        }

        public async Task<bool> AuthorExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Authors.AnyAsync(a => a.Id == id, cancellationToken);
        }
    }
}