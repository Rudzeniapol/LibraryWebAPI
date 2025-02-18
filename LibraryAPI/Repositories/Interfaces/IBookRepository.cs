using LibraryAPI.DTOs;
using LibraryAPI.Models;

namespace LibraryAPI.Repositories.Interfaces
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task<Book?> GetBookByIdAsync(int id);
        Task<Book?> GetBookByISBNAsync(string isbn);
        Task AddBookAsync(Book book);
        Task UpdateBookAsync(Book book);
        Task DeleteBookAsync(int id);
        Task<bool> BookExistsAsync(int id);
        Task<bool> BorrowBookAsync(int bookId, int userId, int days);
        Task<bool> ReturnBookAsync(int bookId, int userId);
        IQueryable<Book> GetBooksQuery();

    }
}