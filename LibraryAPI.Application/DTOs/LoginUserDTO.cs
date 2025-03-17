namespace LibraryAPI.Application.DTOs;

public record LoginUserDTO
{
    public string Username { get; init; } = null!;
    public string Password { get; init; } = null!;
}