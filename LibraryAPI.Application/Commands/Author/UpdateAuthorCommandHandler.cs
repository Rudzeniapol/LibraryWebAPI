using AutoMapper;
using LibraryAPI.Application.Exceptions;
using LibraryAPI.Domain.Interfaces;
using MediatR;

namespace LibraryAPI.Application.Commands.Author;

public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand>
{
    private readonly IAuthorRepository _authorRepository;
    private readonly IMapper _mapper;

    public UpdateAuthorCommandHandler(IAuthorRepository authorRepository, IMapper mapper)
    {
        _authorRepository = authorRepository;
        _mapper = mapper;
    }

    public async Task Handle(UpdateAuthorCommand command, CancellationToken cancellationToken = default)
    {
        var existingAuthor = await _authorRepository.GetByIdAsync(command.Id, cancellationToken);
        if (existingAuthor == null)
        {
            throw new NotFoundException($"Автор с id {command.Id} не найден");
        }
        existingAuthor.FirstName = command.Author.FirstName == string.Empty ? existingAuthor.FirstName : command.Author.FirstName;
        existingAuthor.LastName = command.Author.LastName == string.Empty ? existingAuthor.LastName : command.Author.LastName;
        existingAuthor.Country = command.Author.Country == string.Empty ? existingAuthor.Country : command.Author.Country;
        existingAuthor.DateOfBirth = command.Author.DateOfBirth == DateTime.MinValue ? existingAuthor.DateOfBirth : command.Author.DateOfBirth;
        await _authorRepository.UpdateAsync(existingAuthor, cancellationToken);
    }
}