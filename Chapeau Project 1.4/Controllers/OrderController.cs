using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.Services.Menu;
using Chapeau_Project_1._4.Services.OrderItems;
using Chapeau_Project_1._4.Services.Orders;
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

        
        [HttpPost]
        public IActionResult InputItemDetails (MenuItem menuItem)
        {
         
            OrderItem orderItem = new OrderItem { 
                MenuItem = menuItem, 
                ItemStatus = EItemStatus.NotOrdered,
                OrderNumber = 0 //the order is not send yet so it does not have a number, actually it does have 
                                 //   a number, i need to get it from somewhere :(
                                //check implementation later on an change here and in the repository
            };

            return View(orderItem);

        }

        [HttpPost]
        public IActionResult AddItemsToOrder(OrderItem orderItem)
        {
            _orderItemService.AddOrderItem(orderItem); //add item to Db

            List<OrderItem> orderList = _orderItemService.DisplayOrderItems();
                                  

            return RedirectToAction("DisplayMenu", "Menu", orderList);
        }


        public IActionResult TakeOrder(/*int tableNumber*/)
        {
            //_orderService.AddNewOrder(tableNumber);


            return View();
        }


    }
}
