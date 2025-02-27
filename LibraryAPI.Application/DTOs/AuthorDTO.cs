namespace LibraryAPI.Application.DTOs
{
    public record AuthorDTO
    {
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string Country { get; init; } = string.Empty;
    }
}