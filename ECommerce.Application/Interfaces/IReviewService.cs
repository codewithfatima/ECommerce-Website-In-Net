using ECommerce.Application.DTOs.Reviews;
using ECommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces
{
 
        public interface IReviewService
        {
            Task<Review> CreateReview(CreateReviewDto createReviewDto);

            Task<IEnumerable<Review>> GetAllReviews();
        }


  
}
