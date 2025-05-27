using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.Services.Menu;
using Chapeau_Project_1._4.Services.OrderItems;
using Chapeau_Project_1._4.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Chapeau_Project_1._4.Controllers
{
    public class MenuController : Controller
    {
        private readonly IMenuService _menuService;
        private readonly IOrderItemService _orderItemService;

        public MenuController(IMenuService menuService, IOrderItemService orderItemService)
        {
            _menuService = menuService;
            _orderItemService = orderItemService;
        }

        public IActionResult DisplayMenu(ECardOptions cardFilter, ECategoryOptions categoryFilter)
        {
            List<MenuItem> menu = _menuService.GetMenuItems(cardFilter, categoryFilter);
            var categories = _menuService.GetCardCategories();
            var orderItems = _orderItemService.DisplayOrderItems();     // 

            var viewModel = new MenuViewModel
            {
                CardFilter = cardFilter,
                CategoryFilter = categoryFilter,
                CategoriesDictionary = categories,
                MenuItems = menu,
                OrderItems = orderItems    // 
            };

            return View(viewModel);
        }

        

    }
}
