using System.Collections.Generic;

namespace Models.DTO
{
    public class BookDto
    {
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public int PublicationYear { get; set; }
        public List<int> GenreIds { get; set; }
    }
}
