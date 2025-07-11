using BookShop.API.Models;
using Microsoft.EntityFrameworkCore;
using rememorize.Data;

namespace BookShop.API.Service
{
    public class BuyHistoryRepository : IBuyHistoryRepository
    {
        private readonly AppDbContext _context;
        public BuyHistoryRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task Add(BuyHistory buyHistory)
        {
            await _context.BuyHistories.AddAsync(buyHistory);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<BuyHistory>> GetAll()
        {
            return await _context.BuyHistories.ToListAsync();
        }

        public async Task UpsertBuyHistory(string bookCode, string bookName)
        {
            var existing = await _context.BuyHistories.FirstOrDefaultAsync(x => x.BookCode == bookCode);

            if (existing != null)
            {
                existing.BuyCount++;
                existing.LastBuyDate = DateTime.UtcNow;
            }
            else
            {
                await _context.BuyHistories.AddAsync(new BuyHistory
                {
                    BookCode = bookCode,
                    BookName = bookName,
                    BuyCount = 1,
                    LastBuyDate = DateTime.UtcNow
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }


    }
}
