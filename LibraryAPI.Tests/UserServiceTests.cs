using System;
using System.Threading.Tasks;
using LibraryAPI.Data;
using LibraryAPI.Models;
using LibraryAPI.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LibraryAPI.Tests
{
    public class UserServiceTests : IDisposable
    {
        private readonly LibraryDbContext _context;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new LibraryDbContext(options);
            _userService = new UserService(_context);
        }

        [Fact]
        public async Task RegisterUserAsync_AddsUserToDatabase()
        {
            // Arrange
            string username = "testUser";
            string password = "testPass";
            string role = "user";

            // Act
            var newUser = await _userService.RegisterUserAsync(username, password, role);
            var userFromDb = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            // Assert
            Assert.NotNull(userFromDb);
            Assert.Equal(username, userFromDb.Username);
            // Дополнительно можно проверить хеширование пароля через BCrypt.Verify
        }

        [Fact]
        public async Task GetUserByUsernameAsync_ReturnsCorrectUser()
        {
            // Arrange
            var user = new User { Username = "testUser", PasswordHash = BCrypt.Net.BCrypt.HashPassword("testPass"), Role = "admin" };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act
            var foundUser = await _userService.GetUserByUsernameAsync("testUser");

            // Assert
            Assert.NotNull(foundUser);
            Assert.Equal("admin", foundUser.Role);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
