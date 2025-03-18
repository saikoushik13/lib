using Database;
using Database.Entities;
using DatabaseOperations.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseOperations.Implementation
{
    public class ReviewDatabaseOperations : IReviewDatabaseOperations
    {
        private readonly AppDbContext _context;

        public ReviewDatabaseOperations(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Review>> GetReviewsByBookAsync(string bookISBN)
        {
            return await _context.Reviews
                .Include(r => r.User)
                .Where(r => r.BookISBN == bookISBN)
                .ToListAsync();
        }

        public async Task<Review> GetByIdAsync(int id)
        {
            return await _context.Reviews.FindAsync(id);
        }

        public async Task AddAsync(Review review)
        {
            await _context.Reviews.AddAsync(review);
        }

        public async Task UpdateAsync(Review review)
        {
            _context.Reviews.Update(review);
        }

        public async Task DeleteAsync(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review != null)
            {
                _context.Reviews.Remove(review);
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
