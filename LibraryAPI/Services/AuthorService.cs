using LibraryAPI.Models;
using LibraryAPI.Repositories.Interfaces;
using LibraryAPI.Services.Interfaces;

namespace LibraryAPI.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorService(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<IEnumerable<Author>> GetAllAuthorsAsync(CancellationToken cancellationToken)
        {
            return await _authorRepository.GetAllAuthorsAsync(cancellationToken);
        }

        public async Task<Author?> GetAuthorByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _authorRepository.GetAuthorByIdAsync(id, cancellationToken);
        }

        public async Task AddAuthorAsync(Author author, CancellationToken cancellationToken)
        {
            await _authorRepository.AddAuthorAsync(author, cancellationToken);
        }

        public async Task UpdateAuthorAsync(Author author, CancellationToken cancellationToken)
        {
            await _authorRepository.UpdateAuthorAsync(author, cancellationToken);
        }

        public async Task DeleteAuthorAsync(int id, CancellationToken cancellationToken)
        {
            await _authorRepository.DeleteAuthorAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<Book>> GetBooksByAuthorAsync(int authorId, CancellationToken cancellationToken)
        {
            return await _authorRepository.GetBooksByAuthorAsync(authorId, cancellationToken);
        }
    }
}