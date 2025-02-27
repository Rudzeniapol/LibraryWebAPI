using LibraryAPI.API.Exceptions;
using LibraryAPI.API.Services.Interfaces;
using LibraryAPI.Domain.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
    
    
namespace LibraryAPI.API.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _environment;
        private const string UploadFolder = "uploads";
        private readonly IMemoryCache _cache;
        private readonly IBookRepository _bookRepository;

        public ImageService(IWebHostEnvironment environment, IMemoryCache cache, IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
            _environment = environment; 
            _cache = cache;
        }

        public async Task<string?> UploadImageAsync(IFormFile? file, int id, CancellationToken cancellationToken)
        {
            var book = _bookRepository.GetBookByIdAsync(id, cancellationToken);
            if (book == null)
            {
                throw new NotFoundException($"Книга с id {id} не найдена");
            }
            var bookTitle = book.Result.Title;
            var uploadsFolder = Path.Combine(_environment.WebRootPath, UploadFolder);
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fileName = $"{bookTitle.Replace(" ", "_")}_{Guid.NewGuid()}{Path.GetExtension(file?.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            await using(var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream, cancellationToken);
            }
            
            _cache.Set(fileName, filePath, TimeSpan.FromHours(1));

            return $"/{UploadFolder}/{fileName}";
        }
        
        public Task<byte[]> GetCachedImageAsync(string fileName)
        {
            if (_cache.TryGetValue(fileName, out string? filePath) && File.Exists(filePath))
            {
                return Task.FromResult(File.ReadAllBytes(filePath));
            }
            return Task.FromResult<byte[]>(null);
        }
    }
}