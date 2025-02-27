namespace LibraryAPI.Application.DTOs;

public record RegisterUserDTO
{
    public string Username { get; init; } = null!;
    public string Password { get; init; } = null!;
    public string Role { get; init; } = "user";
}