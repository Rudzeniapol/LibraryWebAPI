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
            _testWebRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot_test");
            if (!Directory.Exists(_testWebRoot))
            {
                Directory.CreateDirectory(_testWebRoot);
            }
            var envMock = new Mock<IWebHostEnvironment>();
            envMock.Setup(m => m.WebRootPath).Returns(_testWebRoot);

            var memoryCache = new MemoryCache(new MemoryCacheOptions());

            _imageService = new ImageService(envMock.Object, memoryCache);
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

            var url = await _imageService.UploadImageAsync(formFile, "TestBook");

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
