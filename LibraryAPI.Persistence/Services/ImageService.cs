using LibraryAPI.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace LibraryAPI.Persistence.Services;

public class ImageService : IImageService
{
    private readonly string _imageStoragePath;
    public ImageService(string webImageStoragePath)
    {
        _imageStoragePath = Path.Combine(webImageStoragePath, "uploads");
        
        if (!Directory.Exists(_imageStoragePath))
        {
            Directory.CreateDirectory(_imageStoragePath);
        }
    }

    public async Task<string> SaveImageAsync(Stream imageStream, IFormFile file, string title)
    {
        var fileName = $"{title.Replace(" ", "_")}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine(_imageStoragePath, fileName);
        
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await imageStream.CopyToAsync(fileStream);
        }

        return $"/uploads/{fileName}";
    }

    public Task<Stream> GetImageAsync(string fileName)
    {
        var filePath = Path.Combine(_imageStoragePath, fileName);

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("Изображение не найдено");
        }
        
        var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        return Task.FromResult((Stream)fileStream);
    }

    public Task DeleteImageAsync(string fileName)
    {
        var filePath = Path.Combine(_imageStoragePath, fileName);

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        return Task.CompletedTask;
    }
}