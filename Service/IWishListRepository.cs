using BookShop.API.Models;

namespace BookShop.API.Service
{
    public interface IWishListRepository
    {
        Task<IEnumerable<WishList>> GetAll();

        Task AddToWish(WishList wishList);

        Task<WishList?> ChekExist(string code);

        Task<bool> SaveChangesAsync();
    }
}
