﻿using LibraryAPI.Application.Exceptions;
using LibraryAPI.Domain.Interfaces;
using LibraryAPI.Persistence.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace LibraryAPI.Application.Commands.Book;

public class UploadBookImageCommand : IRequest<string>
{
    public int BookId { get; set; }
    public IFormFile File { get; set; }
}