namespace LibraryAPI.Application.DTOs
{
    public record BookDTO
    {
        public string ISBN { get; init; } = string.Empty;
        public string Title { get; init; } = string.Empty;
        public string Genre { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public int AuthorId { get; init; }
        public string? ImageUrl { get; init; } = null;
    }
}