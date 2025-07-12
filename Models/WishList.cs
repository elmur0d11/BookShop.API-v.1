using System.ComponentModel.DataAnnotations;

namespace BookShop.API.Models
{
    public class WishList
    {
        public int Id { get; set; }

        [Required]
        public string BookName { get; set; } = string.Empty;

        [Required]
        public string Author { get; set; } = string.Empty;

        [Required]
        public string BookCode { get; set; } = string.Empty;

        [Required]
        public int Quantity { get; set; }
    }
}
