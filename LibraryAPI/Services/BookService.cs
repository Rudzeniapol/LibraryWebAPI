using LibraryAPI.Models;
using LibraryAPI.Repositories.Interfaces;
using LibraryAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using LibraryAPI.Data;
using LibraryAPI.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;

namespace LibraryAPI.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        
        public BookService(IBookRepository bookRepository, LibraryDbContext context)
        {
            _bookRepository = bookRepository;
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync(CancellationToken cancellationToken)
        {
            return await _bookRepository.GetAllBooksAsync(cancellationToken);
        }

        public async Task<Book?> GetBookByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _bookRepository.GetBookByIdAsync(id, cancellationToken);
        }

        public async Task<Book?> GetBookByISBNAsync(string isbn, CancellationToken cancellationToken)
        {
            return await _bookRepository.GetBookByISBNAsync(isbn, cancellationToken);
        }

        public async Task AddBookAsync(Book book, CancellationToken cancellationToken)
        {
            await _bookRepository.AddBookAsync(book, cancellationToken);
        }

        public async Task UpdateBookAsync(Book book, CancellationToken cancellationToken)
        {
            await _bookRepository.UpdateBookAsync(book, cancellationToken);
        }

        public async Task DeleteBookAsync(int id, CancellationToken cancellationToken)
        {
            await _bookRepository.DeleteBookAsync(id, cancellationToken);
        }
        
        public async Task BorrowBookAsync(int bookId, int userId, int days, CancellationToken cancellationToken)
        {
            await _bookRepository.BorrowBookAsync(bookId, userId, days, cancellationToken);
            
        }

        public async Task ReturnBookAsync(int bookId, int userId, CancellationToken cancellationToken)
        {
            await _bookRepository.ReturnBookAsync(bookId, userId, cancellationToken);
        }
        public IQueryable<Book> GetBooksQuery()
        {
            return _bookRepository.GetBooksQuery();
        }

    }
}