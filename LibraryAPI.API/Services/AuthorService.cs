using LibraryAPI.API.Exceptions;
using LibraryAPI.API.Services.Interfaces;
using LibraryAPI.Domain.Models;
using LibraryAPI.Domain.Interfaces;

namespace LibraryAPI.API.Services
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
            var author = await _authorRepository.GetAuthorByIdAsync(id, cancellationToken);
            if (author == null)
            {
                throw new NotFoundException($"Автор с id \"{id}\" не найден");
            }
            return author;
        }

        public async Task AddAuthorAsync(Author author, CancellationToken cancellationToken)
        {
            var existingAuthor = await _authorRepository.GetAuthorByIdAsync(author.Id, cancellationToken);
            if (existingAuthor != null)
            {
                throw new EntityExistsException("Данный автор уже существует");
            }
            await _authorRepository.AddAuthorAsync(author, cancellationToken);
        }

        public async Task UpdateAuthorAsync(Author author, CancellationToken cancellationToken)
        {
            var existingAuthor = await _authorRepository.GetAuthorByIdAsync(author.Id, cancellationToken);
            if (existingAuthor == null)
            {
                throw new NotFoundException($"Автор с id \"{author.Id}\" не найден");
            }
            await _authorRepository.UpdateAuthorAsync(author, cancellationToken);
        }

        public async Task DeleteAuthorAsync(int id, CancellationToken cancellationToken)
        {
            var author = await _authorRepository.GetAuthorByIdAsync(id, cancellationToken);
            if (author == null)
            {
                throw new NotFoundException($"Автор с id \"{id}\" не найден");
            }
            await _authorRepository.DeleteAuthorAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<Book>> GetBooksByAuthorAsync(int authorId, CancellationToken cancellationToken)
        {
            var author = await _authorRepository.GetAuthorByIdAsync(authorId, cancellationToken);
            if (author == null)
            {
                throw new NotFoundException($"Автор с id \"{authorId}\" не найден");
            }
            return await _authorRepository.GetBooksByAuthorAsync(authorId, cancellationToken);
        }
    }
}