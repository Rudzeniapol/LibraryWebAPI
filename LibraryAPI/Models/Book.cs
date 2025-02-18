using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models
{
    public class Book
    {
        public int Id { get; set; }
        
        [Required, MaxLength(17)]
        public string ISBN { get; set; } = string.Empty;
        
        [Required, MaxLength(255)]
        public string Title { get; set; } = string.Empty;
        
        [Required, MaxLength(100)]
        public string Genre { get; set; } = string.Empty;
        
        [Required]
        public string Description { get; set; } = string.Empty;

        public DateTime? BorrowedAt { get; set; }
        public DateTime? ReturnBy { get; set; }
        [Required]  
        public int AuthorId { get; set; }

        public int? UserId { get; set; }
        public string? ImageUrl { get; set; }
    }
}