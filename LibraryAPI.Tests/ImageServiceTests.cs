using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using LibraryAPI.Services;
using LibraryAPI.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Xunit;

namespace LibraryAPI.Tests
{
    public class ImageServiceTests : IDisposable
    {
        private readonly string _testWebRoot;
        private readonly IImageService _imageService;

        public ImageServiceTests()
        {
            // Создаем временную папку для wwwroot
            _testWebRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot_test");
            if (!Directory.Exists(_testWebRoot))
            {
                Directory.CreateDirectory(_testWebRoot);
            }
            // Настраиваем мок IWebHostEnvironment
            var envMock = new Mock<IWebHostEnvironment>();
            envMock.Setup(m => m.WebRootPath).Returns(_testWebRoot);

            // Используем MemoryCache
            var memoryCache = new MemoryCache(new MemoryCacheOptions());

            _imageService = new ImageService(envMock.Object, memoryCache);
        }

        [Fact]
        public async Task UploadImageAsync_ReturnsValidUrl_AndCreatesFile()
        {
            // Arrange: создаем фейковый IFormFile
            var content = "Fake image content";
            var fileName = "test.jpg";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            var formFile = new FormFile(stream, 0, stream.Length, "file", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg"
            };

            // Act
            var url = await _imageService.UploadImageAsync(formFile, "TestBook");

            // Assert
            Assert.False(string.IsNullOrEmpty(url));
            // Проверяем, что файл создан
            string filePath = Path.Combine(_testWebRoot, url.TrimStart('/'));
            Assert.True(File.Exists(filePath));
        }

        public void Dispose()
        {
            // Удаляем временную папку после тестов
            if (Directory.Exists(_testWebRoot))
            {
                Directory.Delete(_testWebRoot, true);
            }
        }
    }
}
