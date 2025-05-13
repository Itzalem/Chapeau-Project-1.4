using Chapeau_Project_1._4.Models;
using Microsoft.AspNetCore.Mvc;

namespace Chapeau_Project_1._4.Controllers
{
    public class MenuController : Controller
    {
        public IActionResult Index()
        {
            List<MenuItem> menu = //method for listing the items;

            return View()   ;
        }
    }
}
