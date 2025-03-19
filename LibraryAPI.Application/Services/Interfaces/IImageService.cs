using Microsoft.AspNetCore.Http;

namespace LibraryAPI.Application.Services.Interfaces;

public interface IImageService
{
    Task<string> SaveImageAsync(Stream imageStream, IFormFile file, string title);
    Task<Stream> GetImageAsync(string fileName);
    Task DeleteImageAsync(string fileName);
}