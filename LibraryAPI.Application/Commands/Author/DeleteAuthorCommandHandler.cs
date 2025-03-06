using LibraryAPI.Application.Exceptions;
using LibraryAPI.Domain.Interfaces;
using MediatR;

namespace LibraryAPI.Application.Commands.Author;

public class DeleteAuthorCommandHandler : IRequestHandler<DeleteAuthorCommand>
{
    private readonly IAuthorRepository _authorRepository;

    public DeleteAuthorCommandHandler(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public async Task Handle(DeleteAuthorCommand command, CancellationToken cancellationToken = default)
    {
        var author = await _authorRepository.GetByIdAsync(command.Id, cancellationToken);
        if (author == null)
        {
            throw new NotFoundException($"Автор с id {command.Id} не найден");
        }
        await _authorRepository.DeleteAsync(author, cancellationToken);
    }
}