using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Domain.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string ISBN { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime? BorrowedAt { get; set; } = null;
        public DateTime? ReturnBy { get; set; } = null;
        public int AuthorId { get; set; }
        public int? UserId { get; set; }
        public string? ImageUrl { get; set; }
    }
}