using LibraryAPI.Domain.Models;

namespace LibraryAPI.Domain.Interfaces
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<Book?> GetBookByISBNAsync(string isbn, CancellationToken cancellationToken = default);
        Task<bool> BookExistsAsync(int id, CancellationToken cancellationToken = default);
        Task BorrowBookAsync(Book book, CancellationToken cancellationToken = default);
        Task ReturnBookAsync(Book book, CancellationToken cancellationToken = default);

        Task<IEnumerable<Book>> GetBooksQuery(int pageNumber, int pageSize, string? genre, string? title,
            CancellationToken cancellationToken = default);

        Task<List<string>> GetOverdueBooks(CancellationToken cancellationToken = default);
    }
}