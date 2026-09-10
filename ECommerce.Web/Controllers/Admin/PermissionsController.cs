using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Web.Controllers.Admin
{
    public class PermissionsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
