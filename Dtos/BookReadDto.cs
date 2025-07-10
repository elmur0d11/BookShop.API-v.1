using System.ComponentModel.DataAnnotations;

namespace rememorize.Dtos
{
    public class BookReadDto
    {
        public int Id { get; set; }
        public string BookName { get; set; } = string.Empty;

        public string Author { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string BookCode { get; set; } = string.Empty;

        public int Quantity { get; set; }
    }
}
