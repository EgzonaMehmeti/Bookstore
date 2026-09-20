namespace BookstoreApi.DTOs
{
    public class BookResponse
    {
        public int BookId { get; set; }

        public AuthorResponse Author { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string? SubTitle { get; set; }
    }
}
