using LibraryAPI.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LibraryAPI.Application.Commands.User;
using LibraryAPI.Application.DTOs;
using LibraryAPI.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.RateLimiting;

namespace LibraryAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableRateLimiting("api")]
    public class AuthController : ControllerBase
    {
        private IMediator _mediator;
        
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command, CancellationToken cancellationToken)
        {
            var newUser = await _mediator.Send(command, cancellationToken);
            return Ok(new { message = "Пользователь зарегистрирован"});
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command, CancellationToken cancellationToken)
        {
            var token = await _mediator.Send(command, cancellationToken);
            return Ok(token);
        }

    }
}
