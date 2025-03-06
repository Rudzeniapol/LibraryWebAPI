using LibraryAPI.Persistence.Services.Interfaces;

namespace LibraryAPI.Persistence.Services;

public class ImageService : IImageService
{
    private readonly string _imageStoragePath;

    public ImageService(string imageStoragePath)
    {
        _imageStoragePath = imageStoragePath;
        
        if (!Directory.Exists(_imageStoragePath))
        {
            Directory.CreateDirectory(_imageStoragePath);
        }
    }

    public async Task<string> SaveImageAsync(Stream imageStream, string fileName)
    {
        var filePath = Path.Combine(_imageStoragePath, fileName);
        
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await imageStream.CopyToAsync(fileStream);
        }

        return filePath;
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