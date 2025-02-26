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

        public async Task<IEnumerable<Author>> GetAllAuthorsAsync()
        {
            return await _authorRepository.GetAllAuthorsAsync();
        }

        public async Task<Author?> GetAuthorByIdAsync(int id) => await _authorRepository.GetAuthorByIdAsync(id);
        public async Task AddAuthorAsync(Author author) => await _authorRepository.AddAuthorAsync(author);
        public async Task UpdateAuthorAsync(Author author) => await _authorRepository.UpdateAuthorAsync(author);
        public async Task DeleteAuthorAsync(int id) => await _authorRepository.DeleteAuthorAsync(id);
        public async Task<IEnumerable<Book>> GetBooksByAuthorAsync(int authorId) => await _authorRepository.GetBooksByAuthorAsync(authorId);
    }
}