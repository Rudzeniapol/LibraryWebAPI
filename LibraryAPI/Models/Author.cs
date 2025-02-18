using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models
{
    public class Author
    {
        public int Id { get; set; }
        
        [Required, MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;
        
        [Required, MaxLength(100)]
        public string LastName { get; set; } = string.Empty;
        
        [Required]
        public DateTime DateOfBirth { get; set; }
        
        [MaxLength(100)]
        public string Country { get; set; } = string.Empty;

        public List<Book> Books { get; set; } = new();
    }
}