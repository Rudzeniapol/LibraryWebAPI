using AutoMapper;
using LibraryAPI.Application.DTOs;
using LibraryAPI.Application.Exceptions;
using LibraryAPI.Domain.Interfaces;
using LibraryAPI.Application.Services.Interfaces;
using MediatR;

namespace LibraryAPI.Application.Commands.Book;

public class UploadBookImageCommandHandler : IRequestHandler<UploadBookImageCommand, ImageUrlDTO>
{
    private readonly IBookRepository _bookRepository;
    private readonly IImageService _imageService;
    private readonly IMapper _mapper;

    public UploadBookImageCommandHandler(IBookRepository bookRepository, IImageService imageService, IMapper mapper)
    {
        _bookRepository = bookRepository;
        _imageService = imageService;
        _mapper = mapper;
    }

    public async Task<ImageUrlDTO> Handle(UploadBookImageCommand request, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetByIdAsync(request.BookId, cancellationToken);
        if (book == null)
        {
            throw new NotFoundException($"Книга с id {request.BookId} не найдена");
        }

        
        var filePath = await _imageService.SaveImageAsync(request.File.OpenReadStream(), request.File, book.Title);

        ImageUrlDTO imageUrlDTO = new ImageUrlDTO
        {
            ImageUrl = filePath
        };
        return imageUrlDTO;
    }
}