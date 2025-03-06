using MediatR;

namespace LibraryAPI.Application.Commands.Author;

public class AddAuthorCommand : IRequest
{
    public Domain.Models.Author Author { get; set; }
}