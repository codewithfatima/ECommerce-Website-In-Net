using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ECommerce.Application.DTOs.Orders;

namespace ECommerce.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        

        public AdminController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
           var token = HttpContext.Session.GetString("JWTToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login" , "Account");
            }
            var client = _httpClientFactory.CreateClient("ECommerceAPI");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var orders = await client.GetFromJsonAsync<List<OrderDto>>("api/order");
            return View(orders);
        }
    }
}
