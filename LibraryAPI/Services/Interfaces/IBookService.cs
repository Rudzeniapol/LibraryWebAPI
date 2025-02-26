using LibraryAPI.Models;

namespace LibraryAPI.Services.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetAllBooksAsync(CancellationToken cancellationToken = default);
        Task<Book?> GetBookByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Book?> GetBookByISBNAsync(string isbn, CancellationToken cancellationToken = default);
        Task AddBookAsync(Book book, CancellationToken cancellationToken = default);
        Task UpdateBookAsync(Book book, CancellationToken cancellationToken = default);
        Task DeleteBookAsync(int id, CancellationToken cancellationToken = default);
        Task BorrowBookAsync(int bookId, int userId, int days, CancellationToken cancellationToken = default);
        Task ReturnBookAsync(int bookId, int userId, CancellationToken cancellationToken = default);
        IQueryable<Book> GetBooksQuery();
    }
}