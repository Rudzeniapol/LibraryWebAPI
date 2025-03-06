using LibraryAPI.Application.Exceptions;
using LibraryAPI.Domain.Interfaces;
using LibraryAPI.Persistence.Services.Interfaces;
using MediatR;

namespace LibraryAPI.Application.Commands.Book;

public class UploadBookImageCommandHandler : IRequestHandler<UploadBookImageCommand, string>
{
    private readonly IBookRepository _bookRepository;
    private readonly IImageService _imageService;

    public UploadBookImageCommandHandler(IBookRepository bookRepository, IImageService imageService)
    {
        _bookRepository = bookRepository;
        _imageService = imageService;
    }

    public async Task<string> Handle(UploadBookImageCommand request, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetByIdAsync(request.BookId, cancellationToken);
        if (book == null)
        {
            throw new NotFoundException($"Книга с id {request.BookId} не найдена");
        }

        var fileName = $"{book.Title.Replace(" ", "_")}_{Guid.NewGuid()}{Path.GetExtension(request.File.FileName)}";
        var filePath = await _imageService.SaveImageAsync(request.File.OpenReadStream(), fileName);

        return $"/uploads/{fileName}";
    }
}