using System.ComponentModel.DataAnnotations;

namespace rememorize.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        public string BookName { get; set; } = string.Empty;

        [Required]
        public string Author { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string BookCode {  get; set; } = string.Empty;

        [Required]
        public int Quantity { get; set; }
    }
}
