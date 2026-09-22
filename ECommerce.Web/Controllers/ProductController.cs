using ECommerce.Application.DTOs.Products;
using ECommerce.Application.DTOs.Reviews;
using ECommerce.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Net.WebSockets;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace ECommerce.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        private bool IsLoggedIn()
        {
            return !string.IsNullOrEmpty(HttpContext.Session.GetString("JWTToken"));
        }
       
        private ClaimsPrincipal? GetUserFromToken()
        {
            var token = HttpContext.Session.GetString("JWTToken");
            
            if(string.IsNullOrEmpty(token))
            {
                return null;
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var identity = new ClaimsIdentity(jwtToken.Claims);
            return new ClaimsPrincipal(identity);
        }

        public async Task<IActionResult> Index(string? category, string? sortBy, string? name, int pageNumber = 1, int pageSize = 12)
        {
            var httpClient = _httpClientFactory.CreateClient("ECommerceAPI");

            var queryParams = new List<string>();

            if (!string.IsNullOrEmpty(category))
            {
                queryParams.Add($"category={category}");
            }
            if (!string.IsNullOrEmpty(name))
            {
                queryParams.Add($"name={name}");
            }
            if (!string.IsNullOrEmpty(sortBy))
            {
                queryParams.Add($"sortBy={sortBy}");
            }

            queryParams.Add($"pageNumber={pageNumber}");
            ViewBag.CurrentPage = pageNumber;

            string  url = "api/products/search";

            if (queryParams.Any())
            {
                url += "?" + string.Join("&", queryParams);
            }
            
            var products = await httpClient.GetFromJsonAsync<List<ProductDto>>(url);
            var totalCount = await httpClient.GetFromJsonAsync<int>($"api/products/search/count?category={category}&name={name}");
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            return View(products);
        }

        public async Task<IActionResult> Details(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("ECommerceAPI");
            var product = await httpClient.GetFromJsonAsync<ProductDto>($"api/products/{id}");
            var allReviews = await httpClient.GetFromJsonAsync<List<Review>>("api/review");
            var productReviews = allReviews!.Where(p => p.ProductId == id).ToList();
            ViewBag.Reviews = productReviews;

            var user = GetUserFromToken();

            if (user != null)
            {
                ViewBag.UserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }

            return View(product);
        }

     

        [HttpPost]
        public async Task<IActionResult> AddReview(CreateReviewDto createReviewDto)
        {
          
                if (!IsLoggedIn())
                {
                return Json(new { requiresLogin = true });
                }


            var httpClient = _httpClientFactory.CreateClient("ECommerceAPI");
            var response = await httpClient.PostAsJsonAsync("api/review", createReviewDto);

            if(response.IsSuccessStatusCode)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddToWishlist(int productId)
        {
            // We will add the logic here next
            var user = GetUserFromToken();
            var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(user == null)
            {
                return Json(new { requiresLogin = true });
            }

            var wishlist = new Whislist
            {
                UserId = userId!,
                ProductId = productId
            };

            var client = _httpClientFactory.CreateClient("ECommerceAPI");
            var response = await client.PostAsJsonAsync("api/wishlist", wishlist);

            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false });
            
        }

    }
    }
