using Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IReviewService
    {
        Task<List<ReviewDto>> GetReviewsByBookAsync(string bookISBN);
        Task AddReviewAsync(int userId, ReviewCreateDto reviewDto);
        Task UpdateReviewAsync(int userId, int reviewId, ReviewUpdateDto reviewDto);
        Task DeleteReviewAsync(int userId, int reviewId);
    }
}
