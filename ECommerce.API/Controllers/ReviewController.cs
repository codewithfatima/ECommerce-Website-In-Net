using ECommerce.Application.DTOs.Reviews;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
        [Route("api/[controller]")]
        [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReview(CreateReviewDto createReviewDto)
        {
          var review = await _reviewService.CreateReview(createReviewDto);
          return Ok(review);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReviews()
        {
         var review  = await _reviewService.GetAllReviews();
         return Ok(review);

        }
    }
}
