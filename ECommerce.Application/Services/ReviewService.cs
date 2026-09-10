using ECommerce.Application.DTOs.Reviews;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class ReviewService:IReviewService
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewService(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

       
        public async Task<IEnumerable<Review>> GetAllReviews()
        {
           var reviews =  await _reviewRepository.GetReviews();
            return reviews;
       
        }

        public async Task<Review> CreateReview(CreateReviewDto createReviewDto)
        {
            var createReview = new Review
            {
                ProductId = createReviewDto.ProductId,
                CustomerName = createReviewDto.CustomerName,
                StarRating = createReviewDto.StarRating,
                Description = createReviewDto.Description,
            };

            await _reviewRepository.AddAsync(createReview);
            await _reviewRepository.SaveChangesAsync();
            return createReview;
        }
    }
}
