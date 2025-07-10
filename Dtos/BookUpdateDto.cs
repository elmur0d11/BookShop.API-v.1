using System.ComponentModel.DataAnnotations;

namespace rememorize.Dtos
{
    public class BookUpdateDto
    {
        public int Id { get; set; }

        [Required]
        public string BookName { get; set; } = string.Empty;

        [Required]
        public string Author { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;
    }
}
