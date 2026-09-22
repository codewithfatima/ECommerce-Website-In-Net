using ECommerce.Application.DTOs.Auth;
using ECommerce.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Identity.Client;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
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
        public async Task<IActionResult> Profile()
        {
            var token = HttpContext.Session.GetString("JWTToken");
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Account");

            var client = _httpClientFactory.CreateClient("ECommerceAPI");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var profile = await client.GetFromJsonAsync<ProfileDto>("api/auth/profile");

            return View(profile);
        }


        [HttpPost]
        public async Task<IActionResult> Profile(ProfileDto dto, IFormFile profileImageFile )
        {
            var token = HttpContext.Session.GetString("JWTToken");

            if (string.IsNullOrEmpty(token)) return RedirectToAction("login", "Account");

            var client = _httpClientFactory.CreateClient("ECommerceAPI");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            await client.PutAsJsonAsync("api/auth/profile", dto );

            var content = new MultipartFormDataContent();
            var stream = profileImageFile.OpenReadStream();

            var fileContent = new StreamContent(stream);

            content.Add(
                fileContent,
                "profileImageFile",
                profileImageFile.FileName
            );

            TempData["SuccessMessage"] = "Profile updated!";
            return RedirectToAction("Profile");
        }

        [HttpPost]
        [Authorize]
        public IActionResult Logout()
        {
            return RedirectToAction("Index" , "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Wislist()
        {
            var user = GetUserFromToken();
            var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return RedirectToAction("Login");
            }

            var client = _httpClientFactory.CreateClient("ECommerceAPI");

            var wishlist = await client.GetFromJsonAsync<List<Whislist>>(
                $"api/wishlist?userId={userId}");

            return View(wishlist);
        }

    }
}
