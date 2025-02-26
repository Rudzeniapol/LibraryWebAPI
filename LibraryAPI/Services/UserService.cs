using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LibraryAPI.Data;
using LibraryAPI.Models;
using LibraryAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using LibraryAPI.DTOs;
using LibraryAPI.Repositories.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace LibraryAPI.Services
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
            return await _userRepository.GetUserByIdAsync(id, cancellationToken);
        }
        
        public async Task<User?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken = default)
        {
            return await _userRepository.GetUserByUsernameAsync(username, cancellationToken);
        }

        public async Task<User?> RegisterUserAsync(RegisterUserDTO registerUser, CancellationToken cancellationToken)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerUser.Password);
            var existingUser = await _userRepository.GetUserByUsernameAsync(registerUser.Username, cancellationToken);
            if (existingUser != null)
                return null;
            var user = new User
            {
                Username = registerUser.Username,
                PasswordHash = hashedPassword,
                Role = registerUser.Role,
            };
            await _userRepository.AddUserAsync(user, cancellationToken);
            return user;
        }

        public async Task<string?> LoginUserAsync(LoginUserDTO loginUser, CancellationToken cancellationToken = default)
        {
            var existingUser = await _userRepository.GetUserByUsernameAsync(loginUser.Username, cancellationToken);
            if (existingUser == null || !BCrypt.Net.BCrypt.Verify(loginUser.Password, existingUser.PasswordHash))
                return null;
            
            return GenerateJwtToken(existingUser);
        }
        
        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpirationMinutes"])),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}