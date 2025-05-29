using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.Services.Menu;
using Chapeau_Project_1._4.Services.OrderItems;
using Chapeau_Project_1._4.Services.Order;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Drawing;


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
        public IActionResult AddOrder(int tableNumber) //pass only table number because i dont have an order yet, i create it in this method
        {
            Order order = _orderService.GetOrderByTable(tableNumber); //too veryfy if the table already has an order open

            if (order != null)
            {
                return RedirectToAction("TakeOrder", new { orderNumber = order.OrderNumber });
            }

            int newOrderNumber = _orderService.AddNewOrder(tableNumber); 

            return RedirectToAction("TakeOrder", new { orderNumber = newOrderNumber });
        }


        public IActionResult TakeOrder(int orderNumber, ECardOptions cardFilter = ECardOptions.Lunch, ECategoryOptions categoryFilter = ECategoryOptions.All)
        {
            Order order = _orderService.GetOrderByNumber(orderNumber);
           
            order.OrderItems = _orderItemService.DisplayItemsPerOrder(order);

            ViewData["CardFilter"] = cardFilter;
            ViewData["CategoryFilter"] = categoryFilter;
            
            return View(order);
        }


        [HttpPost]
        public IActionResult AddItemsToOrder(OrderItem orderItem)
        {
            if (orderItem.Quantity <= 0)
            {
                ViewData["Error"] = "Enter a valid quantity";
                return View("InputItemDetails", orderItem);
            }

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

        public IActionResult SendOrder(Order order) 
        {
            order.OrderItems = _orderItemService.DisplayItemsPerOrder(order);

            //i pass only the order number because I'm reusing a kitchen method that its already expecting a status
            _orderService.UpdateOrderStatus(EOrderStatus.pending, order.OrderNumber);             

            _orderItemService.UpdateAllItemsStatus(EItemStatus.pending, order);

            TempData["SuccesMessage"] = "Order Sent Successfully";

            return RedirectToAction("TakeOrder", new { orderNumber = order.OrderNumber });
        }
        
    }
}
