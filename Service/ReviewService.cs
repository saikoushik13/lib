using AutoMapper;
using Database.Entities;
using DatabaseOperations.Interface;
using Models.DTO;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewDatabaseOperations _reviewDbOperations;
        private readonly IMapper _mapper;

        public ReviewService(IReviewDatabaseOperations reviewDbOperations, IMapper mapper)
        {
            _reviewDbOperations = reviewDbOperations;
            _mapper = mapper;
        }

        public async Task<List<ReviewDto>> GetReviewsByBookAsync(string bookISBN)
        {
            var reviews = await _reviewDbOperations.GetReviewsByBookAsync(bookISBN);
            return _mapper.Map<List<ReviewDto>>(reviews);
        }

        public async Task AddReviewAsync(int userId, ReviewCreateDto reviewDto)
        {
            var review = new Review
            {
                Content = reviewDto.Content,
                Rating = reviewDto.Rating,
                BookISBN = reviewDto.BookISBN,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            await _reviewDbOperations.AddAsync(review);
            await _reviewDbOperations.SaveChangesAsync();
        }

        public async Task UpdateReviewAsync(int userId, int reviewId, ReviewUpdateDto reviewDto)
        {
            var existingReview = await _reviewDbOperations.GetByIdAsync(reviewId);
            if (existingReview == null || existingReview.UserId != userId)
                throw new Exception("Unauthorized or Review Not Found");

            existingReview.Content = reviewDto.Content;
            existingReview.Rating = reviewDto.Rating;

            await _reviewDbOperations.UpdateAsync(existingReview);
            await _reviewDbOperations.SaveChangesAsync();
        }

        public async Task DeleteReviewAsync(int userId, int reviewId)
        {
            var review = await _reviewDbOperations.GetByIdAsync(reviewId);
            if (review == null || review.UserId != userId)
                throw new Exception("Unauthorized or Review Not Found");

            await _reviewDbOperations.DeleteAsync(reviewId);
            await _reviewDbOperations.SaveChangesAsync();
        }
    }
}
