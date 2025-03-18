using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities
{
    public class BookGenre
    {
        public string BookISBN { get; set; }
        [ForeignKey("BookISBN")]
        public Book Book { get; set; }

        public int GenreId { get; set; }
        [ForeignKey("GenreId")]
        public Genre Genre { get; set; }
    }
}
