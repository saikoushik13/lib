using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Database.Entities
{
    public class Book
    {
        [Key]
        [Required]
        [MaxLength(13)]
        public string ISBN { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; }

        [Required, MaxLength(500)]
        public string Description { get; set; }

        [Required, MaxLength(100)]
        public string Author { get; set; }

        [Required, MaxLength(100)]
        public string Publisher { get; set; }

        [Required]
        public int PublicationYear { get; set; }

        // ✅ Many-to-Many: A book can have multiple genres
        public ICollection<Genre> Genres { get; set; } = new List<Genre>();

        public ICollection<Review> Reviews { get; set; }
    }
}

