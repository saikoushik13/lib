using Database.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabaseOperations.Interface
{
    public interface IReviewDatabaseOperations
    {
        Task<List<Review>> GetReviewsByBookAsync(string bookISBN);
        Task<Review> GetByIdAsync(int id);
        Task AddAsync(Review review);
        Task UpdateAsync(Review review);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();
    }
}
