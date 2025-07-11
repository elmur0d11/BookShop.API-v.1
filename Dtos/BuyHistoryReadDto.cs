using System.ComponentModel.DataAnnotations;

namespace BookShop.API.Dtos
{
    public class BuyHistoryReadDto
    {
        public int Id { get; set; }

        public string BookName { get; set; } = string.Empty;

        public string BookCode { get; set; } = string.Empty;

        public DateTime BuyDate { get; set; }

        public int buyCount { get; set; }

        public DateTime LastBuyDate { get; set; }
    }
}
