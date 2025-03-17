using LibraryAPI.Domain.Models;

namespace LibraryAPI.Domain.Interfaces
{
    public interface IAuthorRepository : IRepository<Author>
    {
        Task<IEnumerable<Book>> GetBooksByAuthorAsync(int authorId, CancellationToken cancellationToken = default);
        Task<Author?> AuthorExistsAsync(Author author, CancellationToken cancellationToken = default);
    }
}