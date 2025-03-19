using LibraryAPI.Application.DTOs;
using LibraryAPI.Application.Exceptions;
using LibraryAPI.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace LibraryAPI.Application.Commands.Book;

public class UploadBookImageCommand : IRequest<ImageUrlDTO>
{
    public int BookId { get; set; }
    public IFormFile File { get; set; }
}