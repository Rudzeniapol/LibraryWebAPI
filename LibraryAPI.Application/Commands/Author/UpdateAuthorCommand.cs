using MediatR;

namespace LibraryAPI.Application.Commands.Author;

public class UpdateAuthorCommand : IRequest
{
    public LibraryAPI.Domain.Models.Author Author { get; set; }
}