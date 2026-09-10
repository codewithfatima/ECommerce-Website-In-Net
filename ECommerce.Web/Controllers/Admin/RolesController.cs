using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Web.Controllers.Admin
{
    public class RolesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
