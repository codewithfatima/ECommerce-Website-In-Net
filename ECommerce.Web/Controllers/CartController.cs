using Microsoft.AspNetCore.Mvc;
using ECommerce.Web.Models;
using System.Text.Json;
using ECommerce.Application.DTOs.Products;
using System.Threading.Tasks;

namespace ECommerce.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private bool IsLoggedIn()
        {
            return !string.IsNullOrEmpty(HttpContext.Session.GetString("JWTToken"));
        }

        public CartController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // =========================
        // CART PAGE
        // =========================
        public IActionResult Index()
        {
            var cart = GetCart();

            return View(cart);
        }


        // ======================================
        // ADD TO CART==
        // ======================================
        [HttpPost]
        public async Task<IActionResult> AddToCart(int ProductId, int Quantity)
        {
            if (!IsLoggedIn())
            {
                return Json(new { requiresLogin = true });
            }

            var httpClient = _httpClientFactory.CreateClient("ECommerceAPI");
          var product = await httpClient.GetFromJsonAsync<ProductDto>($"api/products/{ProductId}");
             
          if( product == null )
            {
                return NotFound();
            }


          var cart = GetCart();
          var exisitingItem = cart.FirstOrDefault(c => c.ProductId == ProductId);
            
         if( exisitingItem != null )
            {
               exisitingItem.Quantity += Quantity;
            }
            else
            {
                var cartItem = new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    ProductPrice = product.Price,
                    ImageUrl = product.ProductImage,
                    Quantity = Quantity
                };
                cart.Add(cartItem);

            }
            SaveCart(cart);
            return Json(new {cartCount = cart.Sum(c=>c.Quantity) });    
        }


        // =========================
        // INCREASE QUANTITY
        // =========================
        [HttpPost]
        public IActionResult IncreaseQuantity(int productId)
        {
            var cart = GetCart();
            var existingProduct = cart.FirstOrDefault(c => c.ProductId == productId);

            if(existingProduct != null) 
            { 
                existingProduct.Quantity += 1;
            }
             
            SaveCart(cart);

            return RedirectToAction("Index");
        }


        // =========================
        // DECREASE QUANTITY
        // =========================
        [HttpPost]
        public async Task<IActionResult> DecreaseQuantity(int productId)
        {
            var cart = GetCart();
            var existingProduct = cart.FirstOrDefault(c=>c.ProductId == productId);

            if (existingProduct != null)
            {

                existingProduct.Quantity -= 1;
                if(existingProduct.Quantity <= 0)
                {
                    cart.Remove(existingProduct);
                }
            }

            SaveCart(cart);
            return RedirectToAction("Index");
        }


        // =========================
        // REMOVE ITEM
        // =========================
        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            var cart = GetCart();
            var product = cart.FirstOrDefault(c => c.ProductId == productId);

            if (product != null)
            {
               cart.Remove(product);
            }
            SaveCart(cart);
            return RedirectToAction("Index");
        }


        // =========================
        // GET CART FROM SESSION
        // =========================
        private List<CartItem> GetCart()
        {
            var cartJson = HttpContext.Session.GetString("Cart");

            if (string.IsNullOrEmpty(cartJson))
            {
                return new List<CartItem>();
            }

            var cart = JsonSerializer.Deserialize<List<CartItem>>(cartJson);

            return cart ?? new List<CartItem>();
        }


        // =========================
        // SAVE CART TO SESSION
        // =========================
        private void SaveCart(List<CartItem> cart)
        {
            var cartJson = JsonSerializer.Serialize(cart);

            HttpContext.Session.SetString("Cart", cartJson);
        }
    }
}