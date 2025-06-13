using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.Services.Menu;
using Chapeau_Project_1._4.Services.Order;
using Chapeau_Project_1._4.Services.OrderItems;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace Chapeau_Project_1._4.Controllers
{
    public class BarController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IOrderItemService _orderItemService;
        private readonly IMenuService _menuService;

        public BarController(IOrderService orderService, IOrderItemService orderItemService, IMenuService menuService)
        {
            _orderService = orderService;
            _orderItemService = orderItemService;
            _menuService = menuService;
        }

        public IActionResult Index()
        {
            return View("RunningOrder");
        }

        [HttpPost]
        public IActionResult UpdateItemStatus(int orderItemId, EItemStatus groupStatus)
        {
            // Call into your repo to update a single item
            _orderItemService.UpdateItemStatus(orderItemId, groupStatus);
            // Redirect back to the running-orders page so the view reloads
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
