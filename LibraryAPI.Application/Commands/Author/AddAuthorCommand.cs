using LibraryAPI.Application.DTOs;
using MediatR;

namespace LibraryAPI.Application.Commands.Author;

public class AddAuthorCommand : IRequest<int>
{
    public ChangeAuthorDTO Author { get; set; }
}