using AutoMapper;
using LibraryAPI.Application.Commands.User;
using LibraryAPI.Application.DTOs;
using LibraryAPI.Application.DTOs.MappingProfiles;
using LibraryAPI.Application.Exceptions;
using LibraryAPI.Persistence.Services.Interfaces;
using LibraryAPI.Domain.Interfaces;
using LibraryAPI.Persistence.DTOs;
using Moq;

namespace LibraryAPI.Tests
{
    public class RegisterUserCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPasswordService> _passwordServiceMock;
        private readonly RegisterUserCommandHandler _handler;
        private readonly IMapper _mapper;

        public RegisterUserCommandHandlerTests()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<UserMappingProfile>();
            });
            _mapper = configuration.CreateMapper();
            _userRepositoryMock = new Mock<IUserRepository>();
            _passwordServiceMock = new Mock<IPasswordService>();
            _handler = new RegisterUserCommandHandler(_userRepositoryMock.Object, _passwordServiceMock.Object, _mapper);
        }

        [Fact]
        public async Task Handle_WhenUserNotExists_ShouldCreateNewUser()
        {
            var request = new RegisterUserCommand
            {
                RegisterUser = new RegisterUserDTO
                {
                    Username = "testuser",
                    Password = "password123",
                    Role = "User"
                }
            };

            _userRepositoryMock.Setup(x => x.GetUserByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Domain.Models.User)null);
            
            _passwordServiceMock.Setup(x => x.HashPassword(It.IsAny<string>()))
                .Returns("hashed_password");
            
            var result = await _handler.Handle(request, CancellationToken.None);
            
            _userRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Domain.Models.User>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.Equal(request.RegisterUser.Username, result.Username);
            Assert.Equal("hashed_password", result.Password);
        }

        [Fact]
        public async Task Handle_WhenUserExists_ShouldThrowException()
        {
            var request = new RegisterUserCommand
            {
                RegisterUser = new RegisterUserDTO { Username = "existinguser" }
            };

            _userRepositoryMock.Setup(x => x.GetUserByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Domain.Models.User());
            
            await Assert.ThrowsAsync<EntityExistsException>(() => _handler.Handle(request, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldCallPasswordServiceWithCorrectPassword()
        {
            var password = "securePassword123";
            var request = new RegisterUserCommand
            {
                RegisterUser = new RegisterUserDTO { Password = password }
            };

            _userRepositoryMock.Setup(x => x.GetUserByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Domain.Models.User)null);
            
            await _handler.Handle(request, CancellationToken.None);
            
            _passwordServiceMock.Verify(x => x.HashPassword(password), Times.Once);
        }
    }

    public class LoginUserCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPasswordService> _passwordServiceMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly LoginUserCommandHandler _handler;

        public LoginUserCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _passwordServiceMock = new Mock<IPasswordService>();
            _tokenServiceMock = new Mock<ITokenService>();
            _handler = new LoginUserCommandHandler(
                _userRepositoryMock.Object,
                _passwordServiceMock.Object,
                _tokenServiceMock.Object);
        }

        [Fact]
        public async Task Handle_WithValidCredentials_ShouldReturnToken()
        {
            var user = new Domain.Models.User
            {
                Username = "testuser",
                PasswordHash = "hashed_password"
            };

            var expectedToken = new TokenDTO("access_token", "refresh_token");
            
            var request = new LoginUserCommand
            {
                LoginUser = new LoginUserDTO
                {
                    Username = "testuser",
                    Password = "correct_password"
                }
            };

            _userRepositoryMock.Setup(x => x.GetUserByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
            
            _passwordServiceMock.Setup(x => x.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);
            
            _tokenServiceMock.Setup(x => x.GenerateJwtToken(It.IsAny<Domain.Models.User>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedToken);
            
            var result = await _handler.Handle(request, CancellationToken.None);
            
            Assert.NotNull(result);
            Assert.Equal(expectedToken.AccessToken, result.AccessToken);
            Assert.Equal(expectedToken.RefreshToken, result.RefreshToken);
            _tokenServiceMock.Verify(x => x.GenerateJwtToken(user, true, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnCorrectTokenStructure()
        {
            var user = new Domain.Models.User
            {
                Username = "testuser",
                PasswordHash = "hashed_password"
            };

            var testToken = new TokenDTO("test_access", "test_refresh");
            
            var request = new LoginUserCommand
            {
                LoginUser = new LoginUserDTO
                {
                    Username = "testuser",
                    Password = "correct_password"
                }
            };

            _userRepositoryMock.Setup(x => x.GetUserByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
            
            _passwordServiceMock.Setup(x => x.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);
            
            _tokenServiceMock.Setup(x => x.GenerateJwtToken(It.IsAny<Domain.Models.User>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(testToken);
            
            var result = await _handler.Handle(request, CancellationToken.None);
            
            Assert.IsType<TokenDTO>(result);
            Assert.False(string.IsNullOrEmpty(result.AccessToken));
            Assert.False(string.IsNullOrEmpty(result.RefreshToken));
        }

        [Fact]
        public async Task Handle_WithInvalidUsername_ShouldThrowException()
        {
            var request = new LoginUserCommand
            {
                LoginUser = new LoginUserDTO { Username = "invaliduser" }
            };

            _userRepositoryMock.Setup(x => x.GetUserByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Domain.Models.User)null);
            
            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(request, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WithInvalidPassword_ShouldThrowException()
        {
            var user = new Domain.Models.User { PasswordHash = "hashed_password" };
            var request = new LoginUserCommand
            {
                LoginUser = new LoginUserDTO { Password = "wrong_password" }
            };

            _userRepositoryMock.Setup(x => x.GetUserByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
            
            _passwordServiceMock.Setup(x => x.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(false);
            
            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(request, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldVerifyPasswordWithCorrectParameters()
        {
            var storedHash = "correct_hash";
            var inputPassword = "user_password";
            var user = new Domain.Models.User { PasswordHash = storedHash };
            
            var request = new LoginUserCommand
            {
                LoginUser = new LoginUserDTO { Password = inputPassword }
            };

            _userRepositoryMock.Setup(x => x.GetUserByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
            
            _passwordServiceMock.Setup(x => x.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);

            await _handler.Handle(request, CancellationToken.None);

            _passwordServiceMock.Verify(x => x.VerifyPassword(storedHash, inputPassword), Times.Once);
        }
    }
}