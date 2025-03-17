using AutoMapper;
using LibraryAPI.Application.DTOs;
using LibraryAPI.Application.Exceptions;
using LibraryAPI.Domain.Interfaces;
using MediatR;

namespace LibraryAPI.Application.Commands.Book;

public class AddBookCommandHandler : IRequestHandler<AddBookCommand, int>
{
     private readonly IBookRepository _bookRepository;
     private readonly IMapper _mapper;

     public AddBookCommandHandler(IBookRepository bookRepository, IMapper mapper)
     {
          _bookRepository = bookRepository;
          _mapper = mapper;
     }

     public async Task<int> Handle(AddBookCommand command, CancellationToken cancellationToken = default)
     {
          var existingBook = await _bookRepository.GetBookByISBNAsync(command.Book.ISBN, cancellationToken);
          if (existingBook != null)
          {
               throw new EntityExistsException($"Книга с ISBN {command.Book.ISBN} уже существует");
          }
          var bookToAdd = _mapper.Map<BookDTO, Domain.Models.Book>(command.Book);
          await _bookRepository.AddAsync(bookToAdd, cancellationToken);
          var createdBook = await _bookRepository.GetBookByISBNAsync(command.Book.ISBN, cancellationToken);
          return createdBook.Id;
     }
}