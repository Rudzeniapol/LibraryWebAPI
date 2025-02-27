using LibraryAPI.Domain.Models;

namespace LibraryAPI.Application.Services.Interfaces
{
    public interface IAuthorService
    {
        Task<IEnumerable<Author>> GetAllAuthorsAsync(CancellationToken cancellationToken = default);
        Task<Author?> GetAuthorByIdAsync(int id, CancellationToken cancellationToken = default);
        Task AddAuthorAsync(Author author, CancellationToken cancellationToken = default);
        Task UpdateAuthorAsync(Author author, CancellationToken cancellationToken = default);
        Task DeleteAuthorAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Book>> GetBooksByAuthorAsync(int authorId, CancellationToken cancellationToken = default);
    }
}