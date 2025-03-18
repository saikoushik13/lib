using Database.Entities;
using System.Threading.Tasks;

namespace DatabaseOperations.Interface
{
    public interface IUserDatabaseOperations
    {
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByIdAsync(int id);
        Task AddAsync(User user);
        Task SaveChangesAsync();
    }
}
