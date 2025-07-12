using Microsoft.EntityFrameworkCore;
using rememorize.Data;
using rememorize.Models;

namespace rememorize.Service
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext _context;
        public BookRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddBook(Book book)
        {
            if (book == null)
                throw new ArgumentNullException(nameof(book));

            await _context.Books.AddAsync(book);
        }

        public void DeleteBook(Book book)
        {
            if (book == null)
                throw new ArgumentNullException(nameof(book));

            _context.Books.Remove(book);
        }

        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<Book> GetBook(int Id)
        {
            return await _context.Books.FirstOrDefaultAsync(Bbook => Bbook.Id == Id);
        }

        public async Task UpdateBook(Book book)
        {
            ///
        }

        public async Task<Book> FindByCode(string code)
        {
            return await _context.Books.FirstOrDefaultAsync(b => b.BookCode == code);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }


    }
}
