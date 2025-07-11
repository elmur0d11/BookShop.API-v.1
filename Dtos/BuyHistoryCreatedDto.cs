using System.ComponentModel.DataAnnotations;

namespace BookShop.API.Dtos
{
    public class BuyHistoryCreatedDto
    {
        [Required]
        public string BookName { get; set; } = string.Empty;

        [Required]
        public string BookCode { get; set; } = string.Empty;

        [Required]
        public DateTime BuyDate { get; set; }

        [Required]
        public int buyCount { get; set; }
    }
}
