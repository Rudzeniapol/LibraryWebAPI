using LibraryAPI.Models;
using LibraryAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LibraryAPI.DTOs;
using LibraryAPI.Exceptions;

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
        public async Task<IActionResult> Register([FromBody] RegisterUserDTO user, CancellationToken cancellationToken)
        {
            if (!(user.Role.ToUpper() == "ADMIN" || user.Role.ToUpper() == "USER"))
            {
                throw new BadRequestException("Невалидная роль");
            }  
            var newUser = await _userService.RegisterUserAsync(user, cancellationToken);
            return Ok(new { message = "Пользователь зарегистрирован", userId = newUser.Id });
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO user, CancellationToken cancellationToken)
        {
            var token = await _userService.LoginUserAsync(user, cancellationToken);
            return Ok(token);
        }

    }
}
