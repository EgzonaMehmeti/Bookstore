using System.ComponentModel.DataAnnotations;

namespace BookstoreApi.DTOs
{
    public class UpdateBookRequest
    {
        [Required]
        public int AuthorId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Title { get; set; } = null!;

        public string? SubTitle { get; set; }
    }
}
