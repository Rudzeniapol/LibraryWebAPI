namespace LibraryAPI.Services.Interfaces
{
    public interface IImageService
    {
        Task<string?> UploadImageAsync(IFormFile file, int id, CancellationToken cancellationToken = default);
    }
}