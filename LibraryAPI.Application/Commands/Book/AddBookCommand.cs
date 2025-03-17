using LibraryAPI.Application.DTOs;
using MediatR;

namespace LibraryAPI.Application.Commands.Book;

public class AddBookCommand : IRequest<int>
{
    public BookDTO Book { get; set; }
}