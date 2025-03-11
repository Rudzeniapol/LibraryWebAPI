using System;
using System.Threading.Tasks;
using AutoMapper;
using LibraryAPI.Application.Commands.User;
using LibraryAPI.Persistence.Data;
using LibraryAPI.Application.DTOs;
using LibraryAPI.Application.DTOs.MappingProfiles;
using LibraryAPI.Application.Queries.User;
using LibraryAPI.Domain.Models;
using LibraryAPI.Persistence.Repositories;
using LibraryAPI.Domain.Interfaces;
using LibraryAPI.Persistence.Services;
using LibraryAPI.Persistence.Services.Interfaces;
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
        private readonly IPasswordService _passwordService;
        private readonly IMapper _mapper;

        public UserServiceTests()
        {
            var conf = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<UserMappingProfile>();
            });
            _mapper = conf.CreateMapper();
            _passwordService = new PasswordService();
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new UserRepository(new LibraryDbContext(options));
            Mock<IConfiguration> configuration = new Mock<IConfiguration>();
        }

        [Fact]
        public async Task RegisterUserAsync_AddsUserToDatabase()
        {
            RegisterUserDTO user = new RegisterUserDTO() {Username = "username", Password = "password", Role = "admin"};

            var handler = new RegisterUserCommandHandler(_context, _passwordService, _mapper);

            RegisterUserCommand command = new RegisterUserCommand();
            
            command.RegisterUser = user;
            
            var newUser = await handler.Handle(command, CancellationToken.None);
            var userFromDb = await _context.GetUserByUsernameAsync(user.Username);

            Assert.NotNull(userFromDb);
            Assert.Equal(user.Username, userFromDb.Username);
        }

        [Fact]
        public async Task GetUserByUsernameAsync_ReturnsCorrectUser()
        {
            var user = new User { Username = "testUser", PasswordHash = BCrypt.Net.BCrypt.HashPassword("testPass"), Role = "admin" };
            await _context.AddAsync(user);

            var handler = new GetUserByUsernameQueryHandler(_context, _mapper);
            
            GetUserByUsernameQuery query = new GetUserByUsernameQuery();
            query.Username = user.Username;
            var foundUser = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(foundUser);
            Assert.Equal("admin", foundUser.Role);
        }
    }
}
