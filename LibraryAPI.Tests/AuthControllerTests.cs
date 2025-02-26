using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibraryAPI.Controllers;
using LibraryAPI.Data;
using LibraryAPI.DTOs;
using LibraryAPI.Models;
using LibraryAPI.Repositories;
using LibraryAPI.Repositories.Interfaces;
using LibraryAPI.Services;
using LibraryAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace LibraryAPI.Tests
{
    public class AuthControllerTests
    {
        private readonly IUserRepository _context;
        private readonly IUserService _userService;
        private readonly AuthController _authController;
        private readonly IConfiguration _configuration;

        public AuthControllerTests()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new UserRepository(new LibraryDbContext(options));
            var inMemorySettings = new Dictionary<string, string?>
            {
                {"Jwt:Key", "super_mega_secret_key_1234567890"},
                {"Jwt:Issuer", "LibraryAPI"},
                {"Jwt:Audience", "LibraryUsers"},
                {"Jwt:ExpirationMinutes", "60"}
            };
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            _userService = new UserService(_context, _configuration);
            _authController = new AuthController(_userService);
        }

        [Fact]
        public async Task Register_ReturnsOk_WhenUserIsRegistered()
        {
            var newUser = new RegisterUserDTO { Username = "testUser", Password = "testPass", Role = "user" };

            var result = await _authController.Register(newUser);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Contains("Пользователь зарегистрирован", okResult.Value?.ToString());
        }

        [Fact]
        public async Task Login_ReturnsToken_WhenCredentialsAreValid()
        {
            var newUser = new User { Username = "testUser1", PasswordHash = "testPass1", Role = "admin" };
            Assert.NotNull(await _userService.RegisterUserAsync(newUser.Username, newUser.PasswordHash, newUser.Role));

            var result = await _authController.Login(new LoginUserDTO { Username = "testUser1", Password = "testPass1"});
            var okResult = Assert.IsType<OkObjectResult>(result);

            var tokenProperty = okResult.Value?.GetType().GetProperty("token");
            Assert.NotNull(tokenProperty); 
            
            var token = tokenProperty.GetValue(okResult.Value) as string;
            Assert.False(string.IsNullOrEmpty(token));
        }
    }
}
