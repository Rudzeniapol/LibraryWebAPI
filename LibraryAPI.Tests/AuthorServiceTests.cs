using AutoMapper;
using LibraryAPI.Application.DTOs.MappingProfiles;
using LibraryAPI.Application.Queries.Author;
using LibraryAPI.Persistence.Data;
using LibraryAPI.Domain.Models;
using LibraryAPI.Domain.Interfaces;
using LibraryAPI.Persistence.Services;
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
        private readonly IMapper _mapper;

        public AuthorServiceTests()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AuthorMappingProfile>();
            });
            _mapper = configuration.CreateMapper();
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

            var handler = new GetAuthorsQueryHandler(_mockRepo.Object, _mapper);
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

            var handler = new GetAuthorByIdQueryHandler(_mockRepo.Object, _mapper);
            GetAuthorByIdQuery query = new GetAuthorByIdQuery();
            query.Id = 1;
            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal("Huxley", result.LastName);
        }
    }
}