using LibraryAPI.Persistence.Data;
using LibraryAPI.Domain.Models;
using LibraryAPI.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LibraryAPI.Tests
{
    public class BookRepositoryTests
    {
        private readonly LibraryDbContext _context;
        private readonly BookRepository _repository;
        
        public BookRepositoryTests()
        {
            _context = GetDbContext();
            _repository = new BookRepository(_context);

            _context.Books.Add(new Book { Id = 1, Title = "1984", Genre = "Dystopia", ISBN = "12345", Description = "A classic novel" });
            _context.Books.Add(new Book { Id = 2, Title = "Brave New World", Genre = "Sci-Fi", ISBN = "67890", Description = "A futuristic novel" });
            _context.SaveChanges();
        }

        private LibraryDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new LibraryDbContext(options);
        }

        
        [Fact]
        public async Task GetAllBooksAsync_ReturnsAllBooks()
        {
            var books = await _repository.GetAllAsync();
            Assert.Equal(2, books.Count());
        }

        [Fact]
        public async Task GetBookByIdAsync_ReturnsCorrectBook()
        {
            var book = await _repository.GetByIdAsync(1);
            Assert.NotNull(book);
            Assert.Equal("1984", book.Title);
        }

        [Fact]
        public async Task AddBookAsync_AddsBookToDatabase()
        {
            var newBook = new Book { Id = 3, Title = "Fahrenheit 451", Genre = "Dystopia", ISBN = "11111", Description = "A book about censorship" };

            await _repository.AddAsync(newBook);
            var book = await _repository.GetByIdAsync(3);

            Assert.NotNull(book);
            Assert.Equal("Fahrenheit 451", book.Title);
        }

        [Fact]
        public async Task DeleteBookAsync_RemovesBookFromDatabase()
        {
            var book = await _repository.GetByIdAsync(1);
            await _repository.DeleteAsync(book!);
            var deletedBook = await _repository.GetByIdAsync(1);
            Assert.Null(deletedBook);
        }
    }
}
