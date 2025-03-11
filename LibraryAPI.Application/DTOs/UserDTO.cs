namespace LibraryAPI.Application.DTOs;

public record UserDTO()
{
    public string Username { get; init; }
    public string Role { get; init; }
    public List<BookDTO> Books { get; init; }
};