using BookShop.API.Models;
using Microsoft.EntityFrameworkCore;
using rememorize.Data;

namespace BookShop.API.Service
{
    public class WishListRepository : IWishListRepository
    {
        private readonly AppDbContext _context;
        public WishListRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddToWish(WishList wishList)
        {
            await _context.WishLists.AddAsync(wishList);
            await _context.SaveChangesAsync();
        }

        public async Task<WishList?> ChekExist(string code)
        {
            return await _context.WishLists.FirstOrDefaultAsync(x => x.BookCode == code);
        }

        public async Task<IEnumerable<WishList>> GetAll()
        {
            return await _context.WishLists.ToListAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}
