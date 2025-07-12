using System.ComponentModel.DataAnnotations;

namespace BookShop.API.Dtos
{
    public class WishListCreatedDto
    {
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
