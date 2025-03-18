using Database.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabaseOperations.Interface
{
    public interface IBookDatabaseOperations
    {
        Task<List<Book>> GetAllAsync();
        Task<List<Book>> GetBooksByDynamicQueryAsync(string dsql);
        Task<Book> GetByISBNAsync(string isbn);
        Task AddAsync(Book book);
        Task UpdateAsync(Book book);
        Task DeleteAsync(string isbn);
        Task SaveChangesAsync();
    }
}
