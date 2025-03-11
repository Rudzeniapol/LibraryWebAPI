using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LibraryAPI.Application.Commands.Book;
using LibraryAPI.Persistence.Data;
using LibraryAPI.Application.DTOs;
using LibraryAPI.Application.DTOs.MappingProfiles;
using LibraryAPI.Application.Queries.Book;
using LibraryAPI.Domain.Models;
using LibraryAPI.Persistence.Repositories;
using LibraryAPI.Persistence.Services;
using LibraryAPI.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LibraryAPI.Tests
{
    public class BookServiceTests : IDisposable
    {
        private readonly LibraryDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBookRepository _bookRepository;

        public BookServiceTests()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<BookMappingProfile>();
            });
            _mapper = configuration.CreateMapper();
            _context = new LibraryDbContext(options);
            _bookRepository = new BookRepository(_context);
            var userRepository = new UserRepository(_context);
        }

        [Fact]
        public async Task AddBookAsync_AddsBookToDatabase()
        {
            var author = new Author { Id = 1, FirstName = "George", LastName = "Orwell" };
            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();

            var book = new BookDTO
            { 
                Title = "1984", 
                Genre = "Dystopia", 
                ISBN = "12345", 
                Description = "A classic novel", 
                AuthorId = author.Id 
            };

            var handler = new AddBookCommandHandler(_bookRepository, _mapper);

            AddBookCommand command = new AddBookCommand();
            
            command.Book = book;
            
            await handler.Handle(command, CancellationToken.None);

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

            var handler = new GetAllBooksQueryHandler(_bookRepository, _mapper);
            
            GetAllBooksQuery query = new GetAllBooksQuery();
            
            var books = await handler.Handle(query, CancellationToken.None);

            Assert.Equal(2, books.Count());
        }

        [Fact]
        public async Task GetBookByIdAsync_ReturnsCorrectBook()
        {
            var book = new Book { Id = 1, Title = "1984", Genre = "Dystopia", ISBN = "12345", Description = "A classic novel", AuthorId = 1 };
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            var handler = new GetBookByIdQueryHandler(_bookRepository, _mapper);
            
            GetBookByIdQuery query = new GetBookByIdQuery { Id = book.Id };
            
            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal("1984", result.Title);
        }

        [Fact]
        public async Task UpdateBookAsync_UpdatesBookDetails()
        {
            var book = new Book 
            { 
                Id = 1,
                Title = "1984", 
                Genre = "Dystopia", 
                ISBN = "12345", 
                Description = "A classic novel", 
                AuthorId = 1 
            };
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            var updatedBook = new BookDTO
            { 
                Title = "1984 - Updated", 
                Genre = "Classic", 
                ISBN = "12345", 
                Description = "Updated description", 
                AuthorId = 1 
            };

            var handler = new UpdateBookCommandHandler(_bookRepository, _mapper);
    
            UpdateBookCommand command = new UpdateBookCommand()
            {
                Book = updatedBook,
                Id = book.Id
            };

            await handler.Handle(command, CancellationToken.None);
            
            var result = await _context.Books.FirstOrDefaultAsync(b => b.Id == book.Id);
            Assert.NotNull(result);
            Assert.Equal("1984 - Updated", result.Title);
            Assert.Equal("Classic", result.Genre);
            Assert.Equal("Updated description", result.Description);
            Assert.Equal("12345", result.ISBN);
            Assert.Equal(1, result.AuthorId);
        }

        [Fact]
        public async Task DeleteBookAsync_RemovesBookFromDatabase()
        {
            var book = new Book { Id = 1, Title = "1984", Genre = "Dystopia", ISBN = "12345", Description = "A classic novel", AuthorId = 1 };
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            var handler = new DeleteBookCommandHandler(_bookRepository);
            
            DeleteBookCommand command = new DeleteBookCommand();
            command.bookId = book.Id;           
            
            await handler.Handle(command, CancellationToken.None);
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
