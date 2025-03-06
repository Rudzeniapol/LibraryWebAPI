using MediatR;

namespace LibraryAPI.Application.Commands.Author;

public class DeleteAuthorCommand : IRequest
{
    public int Id { get; set; }
}