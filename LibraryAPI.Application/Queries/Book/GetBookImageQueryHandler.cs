using LibraryAPI.Persistence.Services.Interfaces;
using MediatR;

namespace LibraryAPI.Application.Queries.Book;

public class GetBookImageQueryHandler : IRequestHandler<GetBookImageQuery, Stream>
{
    private readonly IImageService _imageService;

    public GetBookImageQueryHandler(IImageService imageService)
    {
        _imageService = imageService;
    }

    public async Task<Stream> Handle(GetBookImageQuery query, CancellationToken cancellationToken)
    {
        return await _imageService.GetImageAsync(query.Filename);
    }
}