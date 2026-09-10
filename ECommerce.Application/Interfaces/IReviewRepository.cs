using ECommerce.Application.DTOs.Reviews;
using ECommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces
{
    public interface IReviewRepository
    {
        Task<IEnumerable<Review>> GetReviews();
       Task<Review> AddAsync(Review review);
        Task SaveChangesAsync();
    }
}
