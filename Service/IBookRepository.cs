using rememorize.Models;

namespace rememorize.Service
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllBooks();

        Task<Book> GetBook(int Id);

        Task AddBook(Book book);

        void DeleteBook(Book book);

        Task UpdateBook(Book book);

        Task<Book> FindByCode(string code);

        public Task<bool> SaveChangesAsync();
    }
}
