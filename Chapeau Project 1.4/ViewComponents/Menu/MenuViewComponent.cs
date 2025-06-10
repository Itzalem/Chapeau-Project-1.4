using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.Services.Menu;
using Chapeau_Project_1._4.Services.OrderItems;
using Chapeau_Project_1._4.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Chapeau_Project_1._4.ViewComponents.Menu
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly IMenuService _menuService;
        private readonly IOrderItemService _orderItemService;

        public MenuViewComponent(IMenuService menuService, IOrderItemService orderItemService)
        {
            _menuService = menuService;
            _orderItemService = orderItemService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int orderNumber, ECardOptions cardFilter = ECardOptions.Lunch, ECategoryOptions categoryFilter = ECategoryOptions.All)
        {
            var menuItems = _menuService.GetMenuItems(cardFilter, categoryFilter);
            var categories = _menuService.GetCardCategories();

            var viewModel = new MenuViewModel
            {
                CardFilter = cardFilter,
                CategoryFilter = categoryFilter,
                CategoriesDictionary = categories,
                MenuItems = menuItems
            };

            ViewData["OrderNumber"] = orderNumber;

            return View(viewModel);
        }
    }
}



