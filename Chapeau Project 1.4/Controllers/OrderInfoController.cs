using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.Services.Menu;
using Chapeau_Project_1._4.Services.Drinks;
using Microsoft.AspNetCore.Mvc;
using Chapeau_Project_1._4.Services.Order;
using Chapeau_Project_1._4.Services.OrderItems;
using Chapeau_Project_1._4.ViewModel;

namespace Chapeau_Project_1._4.Controllers
{
    public class OrderInfoController : Controller
    {
        private readonly IMenuService _menuService;
        private readonly IDrinkService _drinkService;
        private readonly IOrderService _orderService;
        private readonly IOrderItemService _orderItemService;

        public OrderInfoController(IMenuService menuService, IDrinkService drinkService, IOrderService orderService, IOrderItemService orderItemService)
        {
            _menuService = menuService;
            _drinkService = drinkService;
            _orderService = orderService;
            _orderItemService = orderItemService;
        }

        /*public IActionResult Index(int? table)
        {
            Order? order = _orderService.GetOrderByTable(table);

            List<OrderItem> orderItems = _orderItemService.GetByOrderNumber(order.OrderNumber);

            List<MenuItem> menuItems = new List<MenuItem>();
            List<Drink> drinks = new List<Drink>();

            foreach (OrderItem orderItem in orderItems)
            {
                MenuItem menuItem = _menuService.GetMenuItemById(orderItem.MenuItemId);
                menuItems.Add(menuItem);

                Drink drink = _drinkService.GetDrinks(orderItem.MenuItemId);
            }

            ViewBag.TableNumber = table;

            OrderInfo orderInfo = new OrderInfo(order, orderItems, menuItems, drinks);

            return View(orderInfo);
        }*/
    }
}
