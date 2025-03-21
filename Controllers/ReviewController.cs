﻿// Code Generated by Sidekick is for learning and experimentation purposes only.

using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTO;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Controllers
{
    [Route("api/reviews")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly IValidator<ReviewCreateDto> _reviewValidator;

        public ReviewController(IReviewService reviewService, IValidator<ReviewCreateDto> reviewValidator)
        {
            _reviewService = reviewService;
            _reviewValidator = reviewValidator;
        }

        /// <summary>
        /// Get all reviews for a specific book (Public Access)
        /// </summary>
        [HttpGet("book/{isbn}")]
        public async Task<IActionResult> GetReviewsByBook(string isbn)
        {
            try
            {
                var reviews = await _reviewService.GetReviewsByBookAsync(isbn);
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return StatusCode(500, new { message = "An error occurred while retrieving reviews.", details = ex.Message });
            }
        }

        /// <summary>
        /// Add a new review (Only Authenticated Users)
        /// </summary>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddReview([FromBody] ReviewCreateDto reviewDto)
        {
            var validationResult = _reviewValidator.Validate(reviewDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            try
            {
                int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                await _reviewService.AddReviewAsync(userId, reviewDto);
                return Ok(new { message = "Review added successfully" });
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return StatusCode(500, new { message = "An error occurred while adding the review.", details = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing review (Only the Review Owner)
        /// </summary>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateReview(int id, [FromBody] ReviewUpdateDto reviewDto)
        {
            try
            {
                int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                await _reviewService.UpdateReviewAsync(userId, id, reviewDto);
                return Ok(new { message = "Review updated successfully" });
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return StatusCode(500, new { message = "An error occurred while updating the review.", details = ex.Message });
            }
        }

        /// <summary>
        /// Delete a review (Only the Review Owner)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteReview(int id)
        {
            try
            {
                int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                await _reviewService.DeleteReviewAsync(userId, id);
                return Ok(new { message = "Review deleted successfully" });
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return StatusCode(500, new { message = "An error occurred while deleting the review.", details = ex.Message });
            }
        }
    }
}
