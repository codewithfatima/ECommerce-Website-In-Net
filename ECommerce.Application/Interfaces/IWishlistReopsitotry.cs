using ECommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces
{
    public  interface IWishlistReopsitotry
    {
        //Task<List<Whislist>> GetAllWishlist();

        Task<List<Whislist>> GetWishlistByUser(string userId);
        Task AddProductToWishlist(Whislist wishlist);
        Task<bool> IsProductInWishlist(string userId, int productId);
        Task RemoveProductFromWishlist(int wishlistId);


    }
}
