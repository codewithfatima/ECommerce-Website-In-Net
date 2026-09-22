using ECommerce.Application.Interfaces;
using ECommerce.Domain.Models;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public  class WishlistService:IWishlistService
    {
        private readonly IWishlistReopsitotry _wishlistRepository;
        public WishlistService(IWishlistReopsitotry wishlistRepository)
        {
            _wishlistRepository = wishlistRepository;
        }

        public async Task<List<Whislist>> GetWishlistByUser(string userId)
        {
            var userWishlist = await _wishlistRepository.GetWishlistByUser(userId);
            return userWishlist;
        }
        public async Task AddProductToWishlist(Whislist wishlist)
        {
            var IsInWishlist = await _wishlistRepository.IsProductInWishlist(wishlist.UserId , wishlist.ProductId);

            if (IsInWishlist)
            {
                throw new Exception("Product is already in wishlist.");
            }
            else
            {
                 await _wishlistRepository.AddProductToWishlist(wishlist);
            }


        }
        public async Task<bool> IsProductInWishlist(string userId, int productId)
        {
            return await _wishlistRepository.IsProductInWishlist(userId, productId);
        }
        public async Task RemoveProductFromWishlist(int wishlistId)
        {
            await _wishlistRepository.RemoveProductFromWishlist(wishlistId);
        }
    }
}
