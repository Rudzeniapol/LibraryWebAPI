using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryAPI.API.Data;
using LibraryAPI.Domain.Models;
using LibraryAPI.API.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LibraryAPI.Tests
{
    public class NotificationServiceTests : IDisposable
    {
        private readonly LibraryDbContext _context;
        private readonly NotificationService _notificationService;

        public NotificationServiceTests()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new LibraryDbContext(options);
            _notificationService = new NotificationService(_context);
        }

        [Fact]
        public async Task GetOverdueBooksAsync_ReturnsOverdueNotifications()
        {
            var overdueBook = new Book 
            { 
                Title = "Overdue Book", 
                ReturnBy = DateTime.UtcNow.AddDays(-1), 
                UserId = 1, 
                ISBN = "111", 
                Genre = "Test", 
                Description = "Test" 
            };
            var notOverdueBook = new Book 
            { 
                Title = "On Time Book", 
                ReturnBy = DateTime.UtcNow.AddDays(5), 
                UserId = 1, 
                ISBN = "222", 
                Genre = "Test", 
                Description = "Test" 
            };
            await _context.Books.AddRangeAsync(overdueBook, notOverdueBook);
            await _context.SaveChangesAsync();

            var notifications = await _notificationService.GetOverdueBooksAsync(CancellationToken.None);

            Assert.Single(notifications);
            Assert.Contains("Overdue Book", notifications.First());
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
