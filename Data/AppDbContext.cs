using BookShop.API.Models;
using Microsoft.EntityFrameworkCore;
using rememorize.Models;

namespace rememorize.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<BuyHistory> BuyHistories { get; set; }
        public DbSet<WishList> WishLists { get; set; }
    }
}
