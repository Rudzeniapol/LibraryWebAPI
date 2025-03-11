using LibraryAPI.Application.DTOs;
using MediatR;

namespace LibraryAPI.Application.Commands.Author;

public class UpdateAuthorCommand : IRequest
{
    public int Id { get; set; }
    public ChangeAuthorDTO Author { get; set; }
}