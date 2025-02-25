using LibraryAPI.Models;
using LibraryAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LibraryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if (!(user.Role.ToUpper() == "ADMIN" || user.Role.ToUpper() == "USER"))
            {
                return BadRequest("Некорректная роль.");
            }
            var newUser = await _userService.RegisterUserAsync(user.Username, user.PasswordHash, user.Role);
            return newUser == null ? BadRequest("Данный пользователь уже существует.") : Ok(new { message = "Пользователь зарегистрирован", userId = newUser.Id });
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            var token = await _userService.LoginUserAsync(user.Username, user.PasswordHash);
            return token == null ? Unauthorized("Неверное имя пользователя или пароль.") : Ok(new {token});
        }

    }
}
