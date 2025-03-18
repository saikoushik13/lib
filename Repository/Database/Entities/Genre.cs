using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Database.Entities
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        // ✅ Many-to-Many: A genre can have multiple books
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
