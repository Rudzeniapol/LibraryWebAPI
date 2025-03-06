using LibraryAPI.Application.Commands.Book;
using LibraryAPI.Application.Exceptions;
using LibraryAPI.Domain.Interfaces;
using MediatR;

namespace LibraryAPI.Application.Commands.Author;

public class AddAuthorCommandHandler : IRequestHandler<AddAuthorCommand>
{
    private readonly IAuthorRepository _authorRepository;
    
    public AddAuthorCommandHandler(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }
    
    public async Task Handle(AddAuthorCommand command, CancellationToken cancellationToken = default)
    {
        var existingAuthor = await _authorRepository.GetByIdAsync(command.Author.Id, cancellationToken);
        if (existingAuthor != null)
        {
            throw new EntityExistsException("Данный автор уже существует");
        }
        await _authorRepository.AddAsync(command.Author, cancellationToken);
    }
}