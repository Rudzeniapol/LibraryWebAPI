using Microsoft.AspNetCore.Http;

namespace LibraryAPI.Application.Services.Interfaces
{
    public interface IImageService
    {
        Task<string?> UploadImageAsync(IFormFile file, int id, CancellationToken cancellationToken = default);
    }
}