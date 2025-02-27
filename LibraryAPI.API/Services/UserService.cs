using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LibraryAPI.API.Data;
using LibraryAPI.Domain.Models;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using LibraryAPI.API.DTOs;
using LibraryAPI.API.Exceptions;
using LibraryAPI.API.Services.Interfaces;
using LibraryAPI.Domain.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace LibraryAPI.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<User?> GetUserByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var user =  await _userRepository.GetUserByIdAsync(id, cancellationToken);
            if (user == null)
            {
                throw new NotFoundException($"Пользователь с id \"{id}\" не найден");
            }

            return user;
        }
        
        public async Task<User?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username, cancellationToken);
            if (user == null)
            {
                throw new NotFoundException($"Пользователь с именем \"{username}\" не найден");
            }
            return user;
        }

        public async Task<User?> RegisterUserAsync(RegisterUserDTO registerUser, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetUserByUsernameAsync(registerUser.Username, cancellationToken);
            if (existingUser != null)
                throw new EntityExistsException($"Пользователь с именем \"{registerUser.Username}\" уже существует");
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerUser.Password);
            var user = new User
            {
                Username = registerUser.Username,
                PasswordHash = hashedPassword,
                Role = registerUser.Role,
            };
            await _userRepository.AddUserAsync(user, cancellationToken);
            return user;
        }

        public async Task<TokenDto?> LoginUserAsync(LoginUserDTO loginUser,
            CancellationToken cancellationToken = default)
        {
            var existingUser = await _userRepository.GetUserByUsernameAsync(loginUser.Username, cancellationToken);
            if (existingUser == null || !BCrypt.Net.BCrypt.Verify(loginUser.Password, existingUser.PasswordHash))
                throw new NotFoundException($"Пользователь с именем \"{loginUser.Username}\" не найден");

            return await GenerateJwtToken(existingUser, populateExp: true, cancellationToken);
        }

        private async Task<TokenDto> GenerateJwtToken(User user, bool populateExp, CancellationToken cancellationToken = default)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpirationMinutes"])),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            );

            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            
            if (populateExp)
            {
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(5);
            }
            
            await _userRepository.UpdateUserAsync(user, cancellationToken);
            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            
            return new TokenDto(accessToken, refreshToken);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                
                return Convert.ToBase64String(randomNumber);
            }
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"])),
                ValidateLifetime = false,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"]
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);   
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Некорректный токен");
            }
            
            return principal;
        }

        public async Task<TokenDto> RefreshToken(TokenDto tokenDto, CancellationToken cancellationToken = default)
        {
            var principal = GetPrincipalFromExpiredToken(tokenDto.AccessToken);
            var user = await _userRepository.GetUserByUsernameAsync(principal.Identity.Name, cancellationToken);
            if (user == null || user.RefreshToken != tokenDto.RefreshToken ||
                user.RefreshTokenExpiryTime <= DateTime.Now)
            {   
                throw new BadRequestException("Невалидный токен либо истёк срок действия токена");
            }
            
            return await GenerateJwtToken(user, populateExp: false, cancellationToken);
        }
    }
}