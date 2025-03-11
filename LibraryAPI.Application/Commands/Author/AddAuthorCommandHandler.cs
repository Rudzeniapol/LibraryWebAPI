using AutoMapper;
using LibraryAPI.Application.Commands.Book;
using LibraryAPI.Application.Exceptions;
using LibraryAPI.Domain.Interfaces;
using MediatR;

namespace LibraryAPI.Application.Commands.Author;

public class AddAuthorCommandHandler : IRequestHandler<AddAuthorCommand, int>
{
    private readonly IAuthorRepository _authorRepository;
    private readonly IMapper _mapper;
    
    public AddAuthorCommandHandler(IAuthorRepository authorRepository, IMapper mapper)
    {
        _authorRepository = authorRepository;
        _mapper = mapper;
    }
    
    public async Task<int> Handle(AddAuthorCommand command, CancellationToken cancellationToken = default)
    {
        var newAuthor = _mapper.Map<Domain.Models.Author>(command.Author);
        var existingAuthor = _authorRepository.AuthorExistsAsync(newAuthor, cancellationToken);
        if (existingAuthor.Result != null)
        {
            throw new EntityExistsException("Такой автор уже существует.");
        }
        await _authorRepository.AddAsync(newAuthor, cancellationToken);
        var addedAuthor = _authorRepository.AuthorExistsAsync(newAuthor, cancellationToken);
        return addedAuthor.Result.Id;
    }
}