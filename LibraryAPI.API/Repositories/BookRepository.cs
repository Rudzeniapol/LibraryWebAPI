using LibraryAPI.API.Data;
using LibraryAPI.API.DTOs;
using LibraryAPI.Domain.Models;
using LibraryAPI.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.API.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryDbContext _context;

        public BookRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Books.ToListAsync(cancellationToken);
        }

        public async Task<Book?> GetBookByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Books.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        }

        public async Task<Book?> GetBookByISBNAsync(string isbn, CancellationToken cancellationToken = default)
        {
            return await _context.Books.FirstOrDefaultAsync(b => b.ISBN == isbn, cancellationToken); 
        }


        public async Task AddBookAsync(Book book, CancellationToken cancellationToken = default)
        {
            await _context.Books.AddAsync(book, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateBookAsync(Book book, CancellationToken cancellationToken = default)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteBookAsync(int id, CancellationToken cancellationToken = default)
        {
            var book = await _context.Books.FindAsync(id, cancellationToken);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<bool> BookExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Books.AnyAsync(b => b.Id == id, cancellationToken);
        }


        public async Task<bool> BorrowBookAsync(int bookId, int userId, int days, CancellationToken cancellationToken = default)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == bookId, cancellationToken);
            if (book == null || book.UserId != null) return false;
            book.UserId = userId;
            book.BorrowedAt = DateTime.UtcNow;
            book.ReturnBy = DateTime.UtcNow.AddDays(days);

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
        
        public IQueryable<Book> GetBooksQuery()
        {
            return _context.Books.AsQueryable();
        }
        
        public async Task<bool> ReturnBookAsync(int bookId, int userId, CancellationToken cancellationToken = default)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == bookId, cancellationToken);
            if (book == null || book.UserId != userId) return false;

            book.UserId = null;
            book.BorrowedAt = null;
            book.ReturnBy = null;

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}