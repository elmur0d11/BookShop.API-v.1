using Nest;
using rememorize.Models;

namespace rememorize.Service.ELS
{
    public class ElasticService : IElasticService
    {
        private readonly IElasticClient _client;
        public ElasticService(IElasticClient client)
        {
            _client = client;
        }
        public async Task DeleteBook(int id)
        {
            await _client.DeleteAsync<Book>(id);
        }

        public async Task IndexBook(Book book)
        {
            await _client.IndexDocumentAsync(book);
        }

        public async Task<IEnumerable<Book>> SearchAsync(string keyword)
        {
            var result = await _client.SearchAsync<Book>(s => s
            .Query(q => q
                .MultiMatch(m => m
                    .Fields(f => f
                        .Field(b => b.BookName)
                        .Field(b => b.Author)
                    )
                    .Query(keyword)
                    .Type(TextQueryType.BestFields)
                   )
                ).Size(30)
            );

            return result.Documents;
        }

        public async Task UpdateBook(Book book)
        {
            await _client.IndexDocumentAsync(book);
        }
    }
}
