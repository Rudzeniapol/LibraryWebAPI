using LibraryAPI.Application.DTOs;
using MediatR;

namespace LibraryAPI.Application.Commands.Book;

public class UpdateBookCommand : IRequest
{
    public BookDTO Book;
    public int Id;
}