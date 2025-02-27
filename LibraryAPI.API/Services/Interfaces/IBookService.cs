using LibraryAPI.API.DTOs;
using LibraryAPI.Domain.Models;

namespace LibraryAPI.API.Services.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetAllBooksAsync(CancellationToken cancellationToken = default);
        Task<Book?> GetBookByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Book?> GetBookByISBNAsync(string isbn, CancellationToken cancellationToken = default);
        Task AddBookAsync(BookDTO book, CancellationToken cancellationToken = default);
        Task UpdateBookAsync(BookDTO book, int id, CancellationToken cancellationToken = default);
        Task DeleteBookAsync(int id, CancellationToken cancellationToken = default);
        Task BorrowBookAsync(int bookId, int userId, int days, CancellationToken cancellationToken = default);
        Task ReturnBookAsync(int bookId, int userId, CancellationToken cancellationToken = default);
        IQueryable<Book> GetBooksQuery();
    }
}