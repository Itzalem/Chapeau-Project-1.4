using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.Services.Menu;
using Chapeau_Project_1._4.ViewModel;
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
       
        public IActionResult DisplayMenu(ECardOptions cardFilter, ECategoryOptions categoryFilter)
        {
            List<MenuItem> menu = _menuService.DisplayMenu(cardFilter, categoryFilter);
            var categories = _menuService.GetCardCategories();         
            
            var viewModel = new MenuViewModel
            {
                CardFilter = cardFilter,
                CategoryFilter = categoryFilter,
                CategoriesDictionary = categories,
                MenuItems = menu
            };

            return View(viewModel);
        }

    }
}
