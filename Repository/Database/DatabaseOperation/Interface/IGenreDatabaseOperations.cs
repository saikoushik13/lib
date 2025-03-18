using Database.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabaseOperations.Interface
{
    public interface IGenreDatabaseOperations
    {
        Task<List<Genre>> GetAllAsync();
        Task<Genre> GetByIdAsync(int id);
        Task<List<Genre>> GetGenresByIdsAsync(List<int> genreIds);
        Task AddAsync(Genre genre);
        Task UpdateAsync(Genre genre);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();
    }
}
