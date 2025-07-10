using System.ComponentModel.DataAnnotations;

namespace rememorize.Dtos
{
    public class BookCreatedDto
    {
        [Required]
        public string BookName { get; set; } = string.Empty;

        [Required]
        public string Author { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int Quantity { get; set; }
    }
}
