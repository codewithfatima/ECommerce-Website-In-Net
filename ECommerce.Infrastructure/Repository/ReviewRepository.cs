using ECommerce.Application.DTOs.Reviews;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Models;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repository
{
    public class ReviewRepository:IReviewRepository
    {
        private readonly AppDbContext _context;

        public ReviewRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Review>> GetReviews()
            => await _context.Reviews.ToListAsync();

        public async Task<Review> AddAsync(Review review)
        {
            var result = await _context.Reviews.AddAsync(review);

            return result.Entity;
        }

        public async Task SaveChangesAsync()
            =>await _context.SaveChangesAsync();
    }
}
