using LibraryAPI.Application.Queries.Author;
using LibraryAPI.Persistence.Data;
using LibraryAPI.Domain.Models;
using LibraryAPI.Domain.Interfaces;
using LibraryAPI.Application.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace LibraryAPI.Tests
{
    public class AuthorServiceTests
    {
        private readonly Mock<IAuthorRepository> _mockRepo;
        private readonly LibraryDbContext _context;
        private readonly IMediator _mediator;

        public AuthorServiceTests()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new LibraryDbContext(options);
            _mockRepo = new Mock<IAuthorRepository>();
        }

        [Fact]
        public async Task GetAllAuthorsAsync_ReturnsAuthors()
        {
            var authors = new List<Author> { new Author { Id = 1, FirstName = "George", LastName = "Orwell" } };
            _mockRepo.Setup(repo => repo.GetAllAsync(CancellationToken.None)).ReturnsAsync(authors);

            var handler = new GetAuthorsQueryHandler(_mockRepo.Object);
            GetAuthorsQuery query = new GetAuthorsQuery();
            var result = await handler.Handle(query, CancellationToken.None);

            Assert.Single(result);
            Assert.Equal("George", result.First().FirstName);
        }

        [Fact]
        public async Task GetAuthorByIdAsync_ReturnsAuthor_WhenExists()
        {
            var author = new Author { Id = 1, FirstName = "Aldous", LastName = "Huxley" };
            _mockRepo.Setup(repo => repo.GetByIdAsync(1, CancellationToken.None)).ReturnsAsync(author);

            var handler = new GetAuthorByIdQueryHandler(_mockRepo.Object);
            GetAuthorByIdQuery query = new GetAuthorByIdQuery();
            query.Id = 1;
            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal("Huxley", result.LastName);
        }
    }
}