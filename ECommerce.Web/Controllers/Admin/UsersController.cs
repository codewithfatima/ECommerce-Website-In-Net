using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Web.Controllers.Admin
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
