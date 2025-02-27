using AutoMapper;
using LibraryAPI.Application.DTOs;
using LibraryAPI.Application.Exceptions;
using LibraryAPI.Application.Services.Interfaces;
using LibraryAPI.Domain.Models;
using LibraryAPI.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace LibraryAPI.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        
        public BookService(IBookRepository bookRepository, IMapper mapper)
        {
            _mapper = mapper;
            _bookRepository = bookRepository;
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync(CancellationToken cancellationToken)
        {
            return await _bookRepository.GetAllBooksAsync(cancellationToken);
        }

        public async Task<Book?> GetBookByIdAsync(int id, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.GetBookByIdAsync(id, cancellationToken);
            if (book == null)
            {
                throw new NotFoundException($"Книга с id \"{id}\" не найдена");
            }
            return book;
        }

        public async Task<Book?> GetBookByISBNAsync(string isbn, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.GetBookByISBNAsync(isbn, cancellationToken);
            if (book == null)
            {
                throw new NotFoundException($"книга с ISBN \"{isbn}\" не найдена");
            }
            return book;
        }

        public async Task AddBookAsync(BookDTO book, CancellationToken cancellationToken)
        {
            var bookToAdd = _mapper.Map<BookDTO, Book>(book);
            await _bookRepository.AddBookAsync(bookToAdd, cancellationToken);
        }

        public async Task UpdateBookAsync(BookDTO book, int id, CancellationToken cancellationToken)
        {
            var existingBook = await _bookRepository.GetBookByIdAsync(id, cancellationToken);
            if (existingBook == null)
            {
                throw new NotFoundException($"Книга с id \"{id}\" не найдена");
            }
            _mapper.Map<BookDTO, Book>(book, existingBook);
            await _bookRepository.UpdateBookAsync(existingBook, cancellationToken);
        }

        public async Task DeleteBookAsync(int id, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.GetBookByIdAsync(id, cancellationToken);
            if (book == null)
            {
                throw new NotFoundException($"Книга с id \"{id}\" не найдена");
            }
            await _bookRepository.DeleteBookAsync(id, cancellationToken);
        }
        
        public async Task BorrowBookAsync(int bookId, int userId, int days, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.GetBookByIdAsync(bookId, cancellationToken);
            if (book == null)
            {
                throw new NotFoundException($"Книга с id \"{bookId}\" не найдена");
            }
            await _bookRepository.BorrowBookAsync(bookId, userId, days, cancellationToken);
            
        }

        public async Task ReturnBookAsync(int bookId, int userId, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.GetBookByIdAsync(bookId, cancellationToken);
            if (book == null)
            {
                throw new NotFoundException($"Книга с id \"{bookId}\" не найдена");
            }
            if (book.UserId != userId || book.BorrowedAt == null)
            {
                throw new BadRequestException("Невалидная информация о возвращаемой книге");
            }
            await _bookRepository.ReturnBookAsync(bookId, userId, cancellationToken);
        }
        public async Task<IEnumerable<Book>> GetBooksQueryAsync(int page, int pageSize, string? genre, string? title, CancellationToken cancellationToken)
        {
            var booksQuery = _bookRepository.GetBooksQuery();
            if (!string.IsNullOrEmpty(genre))
                booksQuery = booksQuery.Where(b => b.Genre.Contains(genre));

            if (!string.IsNullOrEmpty(title))
                booksQuery = booksQuery.Where(b => b.Title.Contains(title));

            var books = await booksQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
            var result = _mapper.Map<IEnumerable<Book>>(books);
            
            return result;
        }

    }
}