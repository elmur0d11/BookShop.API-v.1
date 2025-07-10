using rememorize.Models;

namespace rememorize.Service.ELS
{
    public interface IElasticService
    {
        Task IndexBook(Book book); //Create
        Task DeleteBook(int id); //Delete
        Task<IEnumerable<Book>> SearchAsync(string keyword); //Search
        Task UpdateBook(Book book); //Update
    }
}
