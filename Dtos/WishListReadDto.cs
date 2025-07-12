namespace BookShop.API.Dtos
{
    public class WishListReadDto
    {
        public string BookName { get; set; } = string.Empty;

        public string Author { get; set; } = string.Empty;

        public string BookCode { get; set; } = string.Empty;

        public int Quantity { get; set; }
    }
}
