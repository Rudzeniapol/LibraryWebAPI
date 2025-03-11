namespace LibraryAPI.Application.DTOs;

public record ChangeAuthorDTO()
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public DateTime DateOfBirth { get; init; }
    public string Country { get; init; } = string.Empty;
}