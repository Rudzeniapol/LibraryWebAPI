using AutoMapper;
using LibraryAPI.Models;
using LibraryAPI.Repositories.Interfaces;
using LibraryAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using LibraryAPI.Data;
using LibraryAPI.DTOs;
using LibraryAPI.Exceptions;
using LibraryAPI.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;

namespace LibraryAPI.Services
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
        public IQueryable<Book> GetBooksQuery()
        {
            return _bookRepository.GetBooksQuery();
        }

    }
}