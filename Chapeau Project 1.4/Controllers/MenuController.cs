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

        [HttpGet]
        public IActionResult DisplayMenu(string cardFilter, string categoryFilter)
        {
            List<MenuItem> menu = _menuService.DisplayMenu(cardFilter, categoryFilter);

            List<string> dropdownCategories = _menuService.GetCategoriesByCard(cardFilter);

            ViewBag.DropdownCategories = dropdownCategories;
            ViewBag.CardFilter = cardFilter;
            ViewBag.Categories = categoryFilter;           

            return View(menu);
        }

    }
}
