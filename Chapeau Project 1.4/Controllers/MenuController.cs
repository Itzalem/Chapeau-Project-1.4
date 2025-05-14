using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.Services;
using Microsoft.AspNetCore.Mvc;

namespace Chapeau_Project_1._4.Controllers
{
    public class MenuController : Controller
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        public IActionResult DisplayMenu()
        {
            List<MenuItem> menu = _menuService.DisplayMenu();

			return View(menu);
		}
	}
}
