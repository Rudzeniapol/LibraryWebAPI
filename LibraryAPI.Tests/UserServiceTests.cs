using System;
using System.Threading.Tasks;
using LibraryAPI.Persistence.Data;
using LibraryAPI.Application.DTOs;
using LibraryAPI.Domain.Models;
using LibraryAPI.Persistence.Repositories;
using LibraryAPI.Domain.Interfaces;
using LibraryAPI.Application.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Client;
using Moq;
using Xunit;

namespace LibraryAPI.Tests
{
    public class UserServiceTests
    {
        private readonly IUserRepository _context;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new UserRepository(new LibraryDbContext(options));
            Mock<IConfiguration> configuration = new Mock<IConfiguration>();
            _userService = new UserService(_context, configuration.Object);
        }

        [Fact]
        public async Task RegisterUserAsync_AddsUserToDatabase()
        {
            RegisterUserDTO user = new RegisterUserDTO() {Username = "username", Password = "password", Role = "admin"};

            var newUser = await _userService.RegisterUserAsync(user, CancellationToken.None);
            var userFromDb = await _context.GetUserByUsernameAsync(user.Username);

            Assert.NotNull(userFromDb);
            Assert.Equal(user.Username, userFromDb.Username);
        }

        [Fact]
        public async Task GetUserByUsernameAsync_ReturnsCorrectUser()
        {
            var user = new User { Username = "testUser", PasswordHash = BCrypt.Net.BCrypt.HashPassword("testPass"), Role = "admin" };
            await _context.AddUserAsync(user);

            var foundUser = await _userService.GetUserByUsernameAsync("testUser");

            Assert.NotNull(foundUser);
            Assert.Equal("admin", foundUser.Role);
        }
    }
}
