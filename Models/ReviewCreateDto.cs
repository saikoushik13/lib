namespace Models.DTO
{
    public class ReviewCreateDto
    {
        public string Content { get; set; }
        public int Rating { get; set; }
        public string BookISBN { get; set; }
    }
}
