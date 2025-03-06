using LibraryAPI.Application.Exceptions;
using LibraryAPI.Domain.Interfaces;
using MediatR;

namespace LibraryAPI.Application.Commands.Author;

public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand>
{
    private readonly IAuthorRepository _authorRepository;

    public UpdateAuthorCommandHandler(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public async Task Handle(UpdateAuthorCommand command, CancellationToken cancellationToken = default)
    {
        var existingAuthor = await _authorRepository.GetByIdAsync(command.Author.Id, cancellationToken);
        if (existingAuthor == null)
        {
            throw new NotFoundException($"Автор с id {command.Author.Id} не найден");
        }
        await _authorRepository.UpdateAsync(command.Author, cancellationToken);
    }
}