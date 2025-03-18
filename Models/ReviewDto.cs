namespace Models.DTO
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
        public string BookISBN { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; } // To display who wrote the review
    }
}
