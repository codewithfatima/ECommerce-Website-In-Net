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
    public class WishlistRepository:IWishlistReopsitotry
    {
        private readonly AppDbContext _context;

        public WishlistRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Whislist>> GetWishlistByUser(string userId)
        {
            var userWhislist = await _context.Whislists.Where(w => w.UserId == userId).ToListAsync(); 
            return userWhislist;
        }
        public async Task AddProductToWishlist(Whislist wishlist)
        {
            await _context.Whislists.AddAsync(wishlist);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> IsProductInWishlist(string userId, int productId)
        {
            var userWishlist = await _context.Whislists.FirstOrDefaultAsync(w=>w.UserId == userId && w.ProductId == productId);
            if (userWishlist != null)
            {
                return true;
            }
            return false;
        }
        public async Task RemoveProductFromWishlist(int wishlistId)
        {
            var user = await _context.Whislists.FindAsync(wishlistId);
            if (user != null)
            {
                 _context.Remove(user);
                await _context.SaveChangesAsync();
            }
        }


    }
}
