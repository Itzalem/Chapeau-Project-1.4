using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.Services.Menu;
using Chapeau_Project_1._4.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Chapeau_Project_1._4.Views.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {

        private readonly IMenuService _menuService;

        public MenuViewComponent(IMenuService menuService)
        {
            _menuService = menuService;
        }


        //if it does not work return this code to menucontroller 

        public IViewComponentResult Invoke(ECardOptions cardFilter, ECategoryOptions categoryFilter)
        {
            List<MenuItem> menu = _menuService.GetMenuItems(cardFilter, categoryFilter);
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
