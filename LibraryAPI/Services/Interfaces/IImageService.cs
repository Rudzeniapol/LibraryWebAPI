namespace LibraryAPI.Services.Interfaces
{
    public interface IImageService
    {
        Task<string?> UploadImageAsync(IFormFile file, string bookTitle);
    }
}