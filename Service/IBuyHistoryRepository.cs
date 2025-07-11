using BookShop.API.Models;

namespace BookShop.API.Service
{
    public interface IBuyHistoryRepository
    {
        Task Add(BuyHistory buyHistory);

        Task<IEnumerable<BuyHistory>> GetAll();

        Task UpsertBuyHistory(string bookCode, string bookName);

        Task<bool> SaveChangesAsync();
    }
}
