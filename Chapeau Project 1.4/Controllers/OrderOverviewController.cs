using Microsoft.AspNetCore.Mvc;

namespace Chapeau_Project_1._4.Controllers
{
    public class OrderOverviewController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
