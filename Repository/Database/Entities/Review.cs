using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities
{
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; } // Rating between 1 and 5

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Foreign Key to User
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        // Foreign Key to Book
        public string BookISBN { get; set; }
        [ForeignKey("BookISBN")]
        public Book Book { get; set; }
    }
}
