using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using LibraryAPI.Application.Commands.Book;
using LibraryAPI.Application.DTOs.MappingProfiles;
using LibraryAPI.Persistence.Services;
using LibraryAPI.Persistence.Services.Interfaces;
using LibraryAPI.Domain.Models;
using LibraryAPI.Domain.Interfaces;
using LibraryAPI.Persistence.Data;
using LibraryAPI.Persistence.Repositories;
using LibraryAPI.Persistence.Services;
using LibraryAPI.Persistence.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
        private readonly string _uploadsPath;
        private readonly IMapper _mapper;

        public ImageServiceTests()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ImageMappingProfile>();
            });
            _mapper = configuration.CreateMapper();
            _context = new LibraryDbContext(options);
            _bookRepository = new BookRepository(_context);
            
            _testWebRoot = Path.Combine(Path.GetTempPath(), "LibraryAPI_Tests", Guid.NewGuid().ToString());
            Directory.CreateDirectory(_testWebRoot);
            
            var envMock = new Mock<IWebHostEnvironment>();
            envMock.Setup(m => m.WebRootPath).Returns(_testWebRoot);
            envMock.Setup(m => m.EnvironmentName).Returns("Test");
            
            _imageService = new ImageService(envMock.Object.WebRootPath);
            _uploadsPath = Path.Combine(_testWebRoot, "uploads");
        }

        [Fact]
        public async Task UploadImageAsync_ReturnsValidUrl_AndCreatesFile()
        {
            var content = "Fake image content";
            var fileName = "test.jpg";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            
            var formFile = new FormFile(
                baseStream: stream,
                baseStreamOffset: 0,
                length: stream.Length,
                name: "file",
                fileName: fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg"
            };

            var book = new Book { Title = "Test Book" };
            await _bookRepository.AddAsync(book);
            await _context.SaveChangesAsync();

            var handler = new UploadBookImageCommandHandler(_bookRepository, _imageService, _mapper);
            
            var command = new UploadBookImageCommand
            {
                BookId = book.Id,
                File = formFile
            };
            
            var result = await handler.Handle(command, CancellationToken.None);
            
            Assert.NotNull(result);
            Assert.StartsWith("/uploads/", result.ImageUrl);
            Assert.EndsWith(".jpg", result.ImageUrl);

            var fullPath = Path.Combine(_testWebRoot, result.ImageUrl.TrimStart('/'));
            Assert.True(File.Exists(fullPath));
            
            var fileInfo = new FileInfo(fullPath);
            Assert.True(fileInfo.Length > 0);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();

            if (Directory.Exists(_testWebRoot))
            {
                Directory.Delete(_testWebRoot, true);
            }
        }
    }
}