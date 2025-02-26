using LibraryAPI.DTOs;
using LibraryAPI.Models;

namespace LibraryAPI.Repositories.Interfaces
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllBooksAsync(CancellationToken cancellationToken = default);
        Task<Book?> GetBookByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Book?> GetBookByISBNAsync(string isbn, CancellationToken cancellationToken = default);
        Task AddBookAsync(Book book, CancellationToken cancellationToken = default);
        Task UpdateBookAsync(Book book, CancellationToken cancellationToken = default);
        Task DeleteBookAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> BookExistsAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> BorrowBookAsync(int bookId, int userId, int days, CancellationToken cancellationToken = default);
        Task<bool> ReturnBookAsync(int bookId, int userId, CancellationToken cancellationToken = default);
        IQueryable<Book> GetBooksQuery();

    }
}