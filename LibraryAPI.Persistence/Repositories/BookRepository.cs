using LibraryAPI.Persistence.Data;
using LibraryAPI.Domain.Models;
using LibraryAPI.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Persistence.Repositories
{
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        public BookRepository(LibraryDbContext context) : base(context)
        {
        }

        public async Task<Book?> GetBookByISBNAsync(string isbn, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FirstOrDefaultAsync(b => b.ISBN == isbn, cancellationToken); 
        }

        public async Task<bool> BookExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AnyAsync(b => b.Id == id, cancellationToken);
        }
        
        public async Task BorrowBookAsync(Book book, CancellationToken cancellationToken = default)
        {
            _dbSet.Update(book);
            await _context.SaveChangesAsync(cancellationToken);
        }
        
        public async Task ReturnBookAsync(Book book, CancellationToken cancellationToken = default)
        {
            _dbSet.Update(book);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<Book>> GetBooksQuery(int pageNumber, int pageSize, string? genre, string? title, CancellationToken cancellationToken = default)
        {
            var booksQuery = _dbSet.AsQueryable();
            if (!string.IsNullOrEmpty(genre))
                booksQuery = booksQuery.Where(b => b.Genre.Contains(genre));

            if (!string.IsNullOrEmpty(title))
                booksQuery = booksQuery.Where(b => b.Title.Contains(title));
            
            return await booksQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<string>> GetOverdueBooks(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(b => b.ReturnBy < DateTime.UtcNow && b.UserId != null)
                .Select(b => $"Книга '{b.Title}' просрочена! Верните её как можно скорее.")
                .ToListAsync(cancellationToken);
        }
    }
}