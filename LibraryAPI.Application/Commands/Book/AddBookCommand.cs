using LibraryAPI.Application.DTOs;
using MediatR;

namespace LibraryAPI.Application.Commands.Book;

public class AddBookCommand : IRequest
{
    public BookDTO Book { get; set; }
}