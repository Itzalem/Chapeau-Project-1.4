using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.Services.Menu;
using Chapeau_Project_1._4.Services.Order;
using Chapeau_Project_1._4.Services.OrderItems;
using Chapeau_Project_1._4.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Chapeau_Project_1._4.Controllers
{
    public class KitchenController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IOrderItemService _orderItemService;
        private readonly IMenuService _menuService;

        public KitchenController(IOrderService orderService, IOrderItemService orderItemService, IMenuService menuService)
        {
            _orderService = orderService;
            _orderItemService = orderItemService;
            _menuService = menuService;
        }

        public IActionResult Index(string linkTab , string filterValue = "RunningOrders")
        {
            bool isDrink = linkTab != "food";
            var queryResult = new FoodOrDrinkRecognizerViewModel
            {
                IsDrink = isDrink,
                FilterValue = filterValue
            };
            return View("RunningOrder", queryResult);
        }

        [HttpPost]
        public IActionResult UpdateItemStatus(int orderItemId, EItemStatus itemStatus)
        {

            // Call into your repo to update a single item
            _orderItemService.UpdateItemStatus(orderItemId, itemStatus);
            // Redirect back to the running-orders page so the view reloads
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateCourseStatus(int orderNumber, string category, ECategoryStatus categoryCourseStatus)
        {
            _orderItemService.UpdateCourseStatus(orderNumber, category, categoryCourseStatus);
            return RedirectToAction("Index");
        }



        [HttpPost]
        public IActionResult UpdateOrderStatus(int orderNumber, EOrderStatus orderStatus /*string orderStatus*/)
        {
            _orderService.UpdateOrderStatus(orderStatus, orderNumber);
            return RedirectToAction("Index");
        }

        
    }
}
