using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using LibraryAPI.Persistence.Data;
using LibraryAPI.Domain.Models;
using LibraryAPI.Persistence.Repositories;
using LibraryAPI.Domain.Interfaces;
using LibraryAPI.Application.Services;
using LibraryAPI.Application.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using Moq;
using Xunit;

namespace LibraryAPI.Tests
{
    public class ImageServiceTests : IDisposable
    {
        private readonly LibraryDbContext _context;
        private readonly string _testWebRoot;
        private readonly IImageService _imageService;
        private readonly IBookRepository _bookRepository;

        public ImageServiceTests()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _testWebRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            if (!Directory.Exists(_testWebRoot))
            {
                Directory.CreateDirectory(_testWebRoot);
            }

            var context = new LibraryDbContext(options);
            _bookRepository = new BookRepository(context);

            var envMock = new Mock<IWebHostEnvironment>();
            envMock.Setup(m => m.WebRootPath).Returns(_testWebRoot);

            var memoryCache = new MemoryCache(new MemoryCacheOptions());

            _imageService = new ImageService(envMock.Object, memoryCache, _bookRepository);
        }

        [Fact]
        public async Task UploadImageAsync_ReturnsValidUrl_AndCreatesFile()
        {
            var content = "Fake image content";
            var fileName = "test.jpg";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            var formFile = new FormFile(stream, 0, stream.Length, "file", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg"
            };

            var book = new Book
            {
                Id = 1,
                Title = "SomeTitle"
            };
            await _bookRepository.AddBookAsync(book); 
            
            var uploadsFolder = Path.Combine(_testWebRoot, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var url = await _imageService.UploadImageAsync(formFile, 1, CancellationToken.None);

            Assert.False(string.IsNullOrEmpty(url));
            
            string filePath = Path.Combine(_testWebRoot, url.TrimStart('/'));
            Assert.True(File.Exists(filePath));
        }

        public void Dispose()
        {
            if (Directory.Exists(_testWebRoot))
            {
                Directory.Delete(_testWebRoot, true);
            }
        }
    }
}
