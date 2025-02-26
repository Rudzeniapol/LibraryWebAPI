namespace LibraryAPI.DTOs;

public record LoginUserDTO
{
    public string Username { get; set; }
    public string Password { get; set; }
}