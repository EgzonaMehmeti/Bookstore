using System.ComponentModel.DataAnnotations;

namespace BookstoreApi.DTOs
{
    public class CreateAuthorRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; } = null!;
    }
}
