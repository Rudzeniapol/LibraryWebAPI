using LibraryAPI.Persistence.Data;
using LibraryAPI.Domain.Models;
using LibraryAPI.Domain.Interfaces;
using LibraryAPI.Application.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace LibraryAPI.Tests
{
    public class AuthorServiceTests
    {
        private readonly Mock<IAuthorRepository> _mockRepo;
        private readonly LibraryDbContext _context;
        private readonly AuthorService _service;

        public AuthorServiceTests()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new LibraryDbContext(options);
            _mockRepo = new Mock<IAuthorRepository>();
            _service = new AuthorService(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAllAuthorsAsync_ReturnsAuthors()
        {
            var authors = new List<Author> { new Author { Id = 1, FirstName = "George", LastName = "Orwell" } };
            _mockRepo.Setup(repo => repo.GetAllAuthorsAsync(CancellationToken.None)).ReturnsAsync(authors);

            var result = await _service.GetAllAuthorsAsync(CancellationToken.None);

            Assert.Single(result);
            Assert.Equal("George", result.First().FirstName);
        }

        [Fact]
        public async Task GetAuthorByIdAsync_ReturnsAuthor_WhenExists()
        {
            var author = new Author { Id = 1, FirstName = "Aldous", LastName = "Huxley" };
            _mockRepo.Setup(repo => repo.GetAuthorByIdAsync(1, CancellationToken.None)).ReturnsAsync(author);

            var result = await _service.GetAuthorByIdAsync(1, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal("Huxley", result.LastName);
        }
    }
}