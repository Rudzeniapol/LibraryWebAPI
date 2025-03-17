using AutoMapper;
using LibraryAPI.Application.DTOs;
using LibraryAPI.Application.Exceptions;
using LibraryAPI.Domain.Interfaces;
using MediatR;

namespace LibraryAPI.Application.Commands.Book;

public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand>
{
    private readonly IBookRepository _bookRepository;
    private readonly IMapper _mapper;

    public UpdateBookCommandHandler(IBookRepository bookRepository, IMapper mapper)
    {
        _bookRepository = bookRepository;
        _mapper = mapper;
    }

    public async Task Handle(UpdateBookCommand command, CancellationToken cancellationToken = default)
    {
        var existingBook = await _bookRepository.GetByIdAsync(command.Id, cancellationToken);
        if (existingBook == null)
        {
            throw new NotFoundException($"Книга с id {command.Id} не найдена");
        }
        _mapper.Map(command.Book, existingBook);
        await _bookRepository.UpdateAsync(existingBook, cancellationToken);
    }
}