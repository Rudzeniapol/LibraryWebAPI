namespace LibraryAPI.Persistence.Services.Interfaces;

public interface IImageService
{
    Task<string> SaveImageAsync(Stream imageStream, string fileName);
    Task<Stream> GetImageAsync(string fileName);
    Task DeleteImageAsync(string fileName);
}