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

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _bookRepository.GetAllBooksAsync();
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            return await _bookRepository.GetBookByIdAsync(id);
        }

        public async Task<Book?> GetBookByISBNAsync(string isbn)
        {
            return await _bookRepository.GetBookByISBNAsync(isbn);
        }

        public async Task AddBookAsync(Book book)
        {
            await _bookRepository.AddBookAsync(book);
        }

        public async Task UpdateBookAsync(Book book)
        {
            await _bookRepository.UpdateBookAsync(book);
        }

        public async Task DeleteBookAsync(int id)
        {
            await _bookRepository.DeleteBookAsync(id);
        }
        
        public async Task BorrowBookAsync(int bookId, int userId, int days)
        {
            await _bookRepository.BorrowBookAsync(bookId, userId, days);
            
        }

        public async Task ReturnBookAsync(int bookId, int userId)
        {
            await _bookRepository.ReturnBookAsync(bookId, userId);
        }
        public IQueryable<Book> GetBooksQuery()
        {
            return _bookRepository.GetBooksQuery();
        }

    }
}