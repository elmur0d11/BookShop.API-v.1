using System.ComponentModel.DataAnnotations;

namespace BookShop.API.Models
{
    public class BuyHistory
    {
        public int Id { get; set; }

        [Required]
        public string BookName { get; set; } = string.Empty;

        [Required]
        public string BookCode { get; set; } = string.Empty;

        [Required]
        public DateTime BuyDate { get; set; }

        [Required]
        public int BuyCount { get; set; }

        [Required]
        public DateTime LastBuyDate { get; set; }
    }
}
