using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace Chapeau_Project_1._4.Controllers
{
    public class BarController : Controller
    {
        public IActionResult Index()
        {
            
            return View("RunningOrder");
        }


    }
}
