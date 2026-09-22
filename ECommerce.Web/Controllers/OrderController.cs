using ECommerce.Application.DTOs.Orders;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ECommerce.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public OrderController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> MyOrders()
        {
            var token = HttpContext.Session.GetString("JWTToken"); 
            var userId = HttpContext.Session.GetString("userId");

            if(string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            var client = _httpClientFactory.CreateClient("ECommerceAPI");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var orders = await client.GetFromJsonAsync<List<OrderDto>>("api/order/mine");

            return View(orders);
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder(CreateOrderDto dto)
        {
            var token = HttpContext.Session.GetString("JWTToken");

            if(string.IsNullOrWhiteSpace(token))
                return RedirectToAction("Login", "Account");

            var client = _httpClientFactory.CreateClient("ECommerceAPI");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsJsonAsync("api/order" , dto);

            if(response.IsSuccessStatusCode)
            {
                return RedirectToAction("MyOrders");
            }

            return BadRequest();
        }


        [HttpPost]
        public async Task<IActionResult> Cancel(int id)
        {
            var token = HttpContext.Session.GetString("JWTToken");
            if (string.IsNullOrWhiteSpace(token))
                return RedirectToAction("Login", "Account");

            var client = _httpClientFactory.CreateClient("ECommerceAPI");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsJsonAsync( $"api/order/{id}/cancel", new { reason = "" });

            return RedirectToAction("MyOrders");
        }

        [HttpPost]

        public async Task<IActionResult> UpdateStatus(int id, UpdateStatusDto dto)
        {
           
            var token = HttpContext.Session.GetString("JWTToken");
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Account");

            var client = _httpClientFactory.CreateClient("ECommerceAPI");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PutAsJsonAsync($"api/order/{id}/status", dto);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Admin");
            }
            return RedirectToAction("Index", "Home");
        }

    }
}
