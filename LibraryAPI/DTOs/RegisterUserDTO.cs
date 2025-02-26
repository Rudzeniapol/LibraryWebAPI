namespace LibraryAPI.DTOs;

public record RegisterUserDTO
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Role { get; set; } = "user";
}