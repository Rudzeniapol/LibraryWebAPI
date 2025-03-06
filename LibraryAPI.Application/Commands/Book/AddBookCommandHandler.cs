using AutoMapper;
using LibraryAPI.Application.DTOs;
using LibraryAPI.Application.Exceptions;
using LibraryAPI.Domain.Interfaces;
using MediatR;

namespace LibraryAPI.Application.Commands.Book;

public class AddBookCommandHandler : IRequestHandler<AddBookCommand>
{
     private readonly IBookRepository _bookRepository;
     private readonly IMapper _mapper;

     public AddBookCommandHandler(IBookRepository bookRepository, IMapper mapper)
     {
          _bookRepository = bookRepository;
          _mapper = mapper;
     }

     public async Task Handle(AddBookCommand command, CancellationToken cancellationToken = default)
     {
          var bookToAdd = _mapper.Map<BookDTO, Domain.Models.Book>(command.Book);
          var existingBook = await _bookRepository.GetBookByISBNAsync(bookToAdd.ISBN, cancellationToken);
          if (existingBook != null)
          {
               throw new EntityExistsException($"Книга с ISBN {bookToAdd.ISBN} уже существует");
          }
          await _bookRepository.AddAsync(bookToAdd, cancellationToken);
     }
}