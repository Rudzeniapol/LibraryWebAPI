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

            await _bookService.AddBookAsync(book, CancellationToken.None);

            var result = await _context.Books.FirstOrDefaultAsync(b => b.Title == "1984");
            Assert.NotNull(result);
            Assert.Equal("Dystopia", result.Genre);
            Assert.Equal(author.Id, result.AuthorId);
        }

        [Fact]
        public async Task GetAllBooksAsync_ReturnsAllBooks()
        {
            await _context.Books.AddRangeAsync(new List<Book>
            {
                new Book { Title = "1984", Genre = "Dystopia", ISBN = "12345", Description = "A classic novel", AuthorId = 1 },
                new Book { Title = "Brave New World", Genre = "Sci-Fi", ISBN = "67890", Description = "A futuristic novel", AuthorId = 2 }
            });
            await _context.SaveChangesAsync();

            var books = await _bookService.GetAllBooksAsync(CancellationToken.None);

            Assert.Equal(2, books.Count());
        }

        [Fact]
        public async Task GetBookByIdAsync_ReturnsCorrectBook()
        {
            var book = new Book { Id = 1, Title = "1984", Genre = "Dystopia", ISBN = "12345", Description = "A classic novel", AuthorId = 1 };
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            var result = await _bookService.GetBookByIdAsync(1, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal("1984", result.Title);
        }

        [Fact]
        public async Task UpdateBookAsync_UpdatesBookDetails()
        {
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

            _context.Entry(book).State = EntityState.Detached;

            var updatedBook = new Book 
            { 
                Id = book.Id, 
                Title = "1984 - Updated", 
                Genre = "Classic", 
                ISBN = "12345", 
                Description = "Updated description", 
                AuthorId = 1 
            };
            await _bookService.UpdateBookAsync(updatedBook, CancellationToken.None);

            var result = await _context.Books.FirstOrDefaultAsync(b => b.Id == book.Id);
            Assert.NotNull(result);
            Assert.Equal("1984 - Updated", result.Title);
            Assert.Equal("Classic", result.Genre);
        }

        [Fact]
        public async Task DeleteBookAsync_RemovesBookFromDatabase()
        {
            var book = new Book { Id = 1, Title = "1984", Genre = "Dystopia", ISBN = "12345", Description = "A classic novel", AuthorId = 1 };
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            await _bookService.DeleteBookAsync(1, CancellationToken.None);
            var result = await _context.Books.FirstOrDefaultAsync(b => b.Id == 1);

            Assert.Null(result);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
