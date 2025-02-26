using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string ISBN { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime? BorrowedAt { get; set; }
        public DateTime? ReturnBy { get; set; }
        public int AuthorId { get; set; }
        public int? UserId { get; set; }
        public string? ImageUrl { get; set; }
    }
}