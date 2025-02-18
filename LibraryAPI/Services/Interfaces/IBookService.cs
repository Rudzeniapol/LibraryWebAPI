using LibraryAPI.Models;

namespace LibraryAPI.Services.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task<Book?> GetBookByIdAsync(int id);
        Task<Book?> GetBookByISBNAsync(string isbn);
        Task AddBookAsync(Book book);
        Task UpdateBookAsync(Book book);
        Task DeleteBookAsync(int id);
        Task BorrowBookAsync(int bookId, int userId, int days);
        Task ReturnBookAsync(int bookId, int userId);
        IQueryable<Book> GetBooksQuery();
    }
}