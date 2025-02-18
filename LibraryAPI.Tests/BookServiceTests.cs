using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryAPI.Data;
using LibraryAPI.Models;
using LibraryAPI.Repositories;
using LibraryAPI.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LibraryAPI.Tests
{
    public class BookServiceTests : IDisposable
    {
        private readonly LibraryDbContext _context;
        private readonly BookService _bookService;

        public BookServiceTests()
        {
            // Создаём уникальную базу данных для каждого запуска тестов
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new LibraryDbContext(options);
            var bookRepository = new BookRepository(_context);
            _bookService = new BookService(bookRepository, _context);
        }

        [Fact]
        public async Task AddBookAsync_AddsBookToDatabase()
        {
            // Arrange
            var author = new Author { Id = 1, FirstName = "George", LastName = "Orwell" };
            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();

            var book = new Book 
            { 
                Title = "1984", 
                Genre = "Dystopia", 
                ISBN = "12345", 
                Description = "A classic novel", 
                AuthorId = author.Id 
            };

            // Act
            await _bookService.AddBookAsync(book);

            // Assert
            var result = await _context.Books.FirstOrDefaultAsync(b => b.Title == "1984");
            Assert.NotNull(result);
            Assert.Equal("Dystopia", result.Genre);
            Assert.Equal(author.Id, result.AuthorId);
        }

        [Fact]
        public async Task GetAllBooksAsync_ReturnsAllBooks()
        {
            // Arrange
            await _context.Books.AddRangeAsync(new List<Book>
            {
                new Book { Title = "1984", Genre = "Dystopia", ISBN = "12345", Description = "A classic novel", AuthorId = 1 },
                new Book { Title = "Brave New World", Genre = "Sci-Fi", ISBN = "67890", Description = "A futuristic novel", AuthorId = 2 }
            });
            await _context.SaveChangesAsync();

            // Act
            var books = await _bookService.GetAllBooksAsync();

            // Assert
            Assert.Equal(2, books.Count());
        }

        [Fact]
        public async Task GetBookByIdAsync_ReturnsCorrectBook()
        {
            // Arrange
            var book = new Book { Id = 1, Title = "1984", Genre = "Dystopia", ISBN = "12345", Description = "A classic novel", AuthorId = 1 };
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            // Act
            var result = await _bookService.GetBookByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("1984", result.Title);
        }

        [Fact]
        public async Task UpdateBookAsync_UpdatesBookDetails()
        {
            // Arrange: создаем книгу и добавляем её в InMemoryDatabase.
            var book = new Book 
            { 
                Title = "1984", 
                Genre = "Dystopia", 
                ISBN = "12345", 
                Description = "A classic novel", 
                AuthorId = 1 
            };
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            // Отвязываем отслеживаемый объект, чтобы избежать конфликта при обновлении.
            _context.Entry(book).State = EntityState.Detached;

            // Act: создаем новый экземпляр книги с тем же Id и обновленными данными.
            var updatedBook = new Book 
            { 
                Id = book.Id, 
                Title = "1984 - Updated", 
                Genre = "Classic", 
                ISBN = "12345", 
                Description = "Updated description", 
                AuthorId = 1 
            };
            await _bookService.UpdateBookAsync(updatedBook);

            // Assert: получаем книгу из базы данных и проверяем, что данные обновились.
            var result = await _context.Books.FirstOrDefaultAsync(b => b.Id == book.Id);
            Assert.NotNull(result);
            Assert.Equal("1984 - Updated", result.Title);
            Assert.Equal("Classic", result.Genre);
        }

        [Fact]
        public async Task DeleteBookAsync_RemovesBookFromDatabase()
        {
            // Arrange
            var book = new Book { Id = 1, Title = "1984", Genre = "Dystopia", ISBN = "12345", Description = "A classic novel", AuthorId = 1 };
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            // Act
            await _bookService.DeleteBookAsync(1);
            var result = await _context.Books.FirstOrDefaultAsync(b => b.Id == 1);

            // Assert
            Assert.Null(result);
        }

        // Метод Dispose для очистки InMemoryDatabase после каждого теста
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
