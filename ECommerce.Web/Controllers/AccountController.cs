using ECommerce.Application.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.Authorization;
namespace ECommerce.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private ClaimsPrincipal? GetUserFromToken()
        {
            var token = HttpContext.Session.GetString("JWTToken");
            if(string.IsNullOrEmpty(token))
            {
                return null;
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);


            foreach (var claim in jwtToken.Claims)
            {
                System.Diagnostics.Debug.WriteLine($"{claim.Type} = {claim.Value}");
            }

            var identity = new ClaimsIdentity(jwtToken.Claims);
            return new ClaimsPrincipal(identity);
        }

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var httpClient = _httpClientFactory.CreateClient("ECommerceAPI");
            var response = await httpClient.PostAsJsonAsync("api/auth/register", registerDto);

            if(response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Account created succcessfully!";
                return RedirectToAction("Login");
            }
            return View(registerDto);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
           
            var httpClient = _httpClientFactory.CreateClient("ECommerceAPI");
            var response = await httpClient.PostAsJsonAsync("api/auth/login", loginDto);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
                if (result == null || string.IsNullOrEmpty(result.Token))
                {
                    ModelState.AddModelError("", "Login failed. No token received.");
                    return View(loginDto);
                }

                HttpContext.Session.SetString("JWTToken", result.Token);

                TempData["SuccessMessage"] = "Login successful! Welcome back.";
                return RedirectToAction("Index" , "Home");

            }


            ModelState.AddModelError("", "Invalid email or password.");
            return View(loginDto);
        }


        [HttpGet]
        public IActionResult Profile()
        {
            var user = GetUserFromToken();

            if (user == null)
            {
                return RedirectToAction("Login");
            }
            ViewBag.UserName = user.FindFirst(ClaimTypes.Name)?.Value;
            ViewBag.UserEmail = user.FindFirst(ClaimTypes.Email)?.Value;

            return View();
        }
        [HttpPost]
        [Authorize]
        public IActionResult Logout()
        {
            return RedirectToAction("Index" , "Home");
        }

    }
}
