using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.Services.Menu;
using Chapeau_Project_1._4.Services.OrderItems;
using Chapeau_Project_1._4.Services.Order;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;


namespace Chapeau_Project_1._4.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IOrderItemService _orderItemService;

        public OrderController(IOrderService orderService, IOrderItemService orderItemService)
        {
            _orderService = orderService;
            _orderItemService = orderItemService;
        }

        [HttpGet]
        public IActionResult AddOrder(int tableNumber)
        {
            Order order = _orderService.GetOrderByTable(tableNumber);

            if (order != null)
            {
                return RedirectToAction("TakeOrder", new { orderNumber = order.OrderNumber });
            }

            int newOrderNumber = _orderService.AddNewOrder(tableNumber);            

            return RedirectToAction("TakeOrder", new { orderNumber = newOrderNumber });
        }

        public IActionResult TakeOrder(int orderNumber, ECardOptions cardFilter = ECardOptions.Lunch, ECategoryOptions categoryFilter = ECategoryOptions.All)
        {
            // Sólo pasamos a la vista los orderItems; el menú lo carga el ViewComponent
            List <OrderItem> orderItems = _orderItemService.DisplayItemsPerOrder(orderNumber);

            ViewData["CardFilter"] = cardFilter;
            ViewData["CategoryFilter"] = categoryFilter;
            ViewData["OrderNumber"] = orderNumber;

            return View(orderItems);
        }


        [HttpPost]
        public IActionResult AddItemsToOrder(OrderItem orderItem)
        {
            _orderItemService.AddOrderItem(orderItem); //add item to Db

            return RedirectToAction("TakeOrder", new { orderNumber = orderItem.OrderNumber });

        }


        [HttpPost]
        public IActionResult InputItemDetails(MenuItem menuItem, int OrderNumber)
        {
            OrderItem orderItem = new OrderItem
            {
                MenuItem = menuItem,
                ItemStatus = EItemStatus.onHold,
                OrderNumber = OrderNumber,
            };

            return View(orderItem);

        }


        
    }
}
