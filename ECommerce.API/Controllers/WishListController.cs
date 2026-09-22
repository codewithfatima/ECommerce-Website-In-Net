using ECommerce.Application.Interfaces;
using ECommerce.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishListController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;

        public WishListController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        [HttpGet]
        public async Task<IActionResult> GetWishlistByUser(string userId)
        {
            var wishList = await _wishlistService.GetWishlistByUser(userId);
            return Ok(wishList);

        }

        [HttpPost]
        public async Task<IActionResult> AddProductToWishlist(Whislist wishlist)
        {
           await _wishlistService.AddProductToWishlist(wishlist);
            return Ok("Product Added to Wishlist");
        }

        [HttpGet("check")]
        public async Task<IActionResult> IsProductInWishlist(string userId, int productId)
        {
            var isInWishList = await _wishlistService.IsProductInWishlist(userId, productId);
            return Ok(isInWishList);    
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveProductFromWishlist(int wishlistId)
        {
            await _wishlistService.RemoveProductFromWishlist(wishlistId);
            return Ok("Product Removed from Wishlist");
        }


    }
}
