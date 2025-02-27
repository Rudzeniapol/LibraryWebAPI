using LibraryAPI.API.DTOs;
using LibraryAPI.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TokenController : ControllerBase
{
    private readonly IUserService _userService;

    public TokenController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] TokenDto tokenDto)
    {
        var result = await _userService.RefreshToken(tokenDto);
        return Ok(result);
    }
}