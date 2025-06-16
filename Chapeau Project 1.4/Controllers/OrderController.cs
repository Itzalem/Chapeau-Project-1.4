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
            try
            {
                Order order = _orderService.GetOrderByTable(tableNumber); //too veryfy if the table already has an order open

                if (order != null)
                {
                    return RedirectToAction("TakeOrder", new { orderNumber = order.OrderNumber });
                }

                int newOrderNumber = _orderService.AddNewOrder(tableNumber);

                return RedirectToAction("TakeOrder", new { orderNumber = newOrderNumber });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An unexpected error occurred while creating the order, please try again";
                return RedirectToAction("Overview", "RestaurantTable");
            }
        }

        [HttpGet] //shows the general view to take the Order with every item that's already part of it
        public IActionResult TakeOrder(int orderNumber, ECardOptions cardFilter = ECardOptions.Lunch, ECategoryOptions categoryFilter = ECategoryOptions.All)
        {
            try
            {
                Order order = _orderService.GetOrderByNumber(orderNumber);

                order.OrderItems = _orderItemService.DisplayItemsPerOrder(order);

                ViewData["CardFilter"] = cardFilter;
                ViewData["CategoryFilter"] = categoryFilter;

                return View(order);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An unexpected error occurred, please try again";
                return RedirectToAction("Overview", "RestaurantTable");
            }
        }


        [HttpPost] //create a new order item and shows the view for the note and quantity
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

        [HttpPost]
        public IActionResult AddItemsToOrder(OrderItem orderItem)
        {
            if (orderItem.Quantity <= 0)
            {
                ViewData["QuantityError"] = "Enter a valid quantity";
                return View("InputItemDetails", orderItem);
            }

            if (orderItem.MenuItem.Stock < orderItem.Quantity)
            {
                ViewData["InsufficientStockError"] = $"Not enough stock of the desired product, only {orderItem.MenuItem.Stock} available";
                return View("InputItemDetails", orderItem);
            }

            try
            {
                _orderItemService.AddOrderItem(orderItem); //add item to Db
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An unexpected error occurred while adding the item, please try again";
            }
            return RedirectToAction("TakeOrder", new { orderNumber = orderItem.OrderNumber });
        }


        [HttpPost]
        public IActionResult SendOrder(Order order)
        {
            try
            {
                //the list is not loaded from the form, I need to call a service to get the items of the order
                order.OrderItems = _orderItemService.DisplayItemsPerOrder(order); 

                //i pass only the order number because I'm reusing a kitchen method
                _orderService.UpdateOrderStatus(EOrderStatus.pending, order.OrderNumber);

                /*the stock its reduced before the Item status is changed to avoid already
                  sent items to reduce the stock again*/             
                _orderItemService.ReduceItemStock(order);

                //all the items that are on hold are updated to pending
                _orderItemService.UpdateHoldItemsStatus(order);
                
                _orderItemService.CheckDuplicateItems(order); //only for the new pending items 

                TempData["SuccessMessage"] = "Order Sent Successfully";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An unexpected error occurred while sending the order, please try again";
            }

            return RedirectToAction("TakeOrder", new { orderNumber = order.OrderNumber });
        }


        [HttpPost]
        public IActionResult CancelOrder(Order order)
        {
            try
            {
                //the list is not loaded from the form, I need to call a service to get the items of the order
                order.OrderItems = _orderItemService.DisplayItemsPerOrder(order);  

                _orderService.CancelUnsentOrder(order);

                TempData["CancelMessage"] = "Order Canceled";

                return RedirectToAction("Overview", "RestaurantTable");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An unexpected error occurred while canceling the order, please try again";
                return RedirectToAction("TakeOrder", new { orderNumber = order.OrderNumber });
            }

        }


        [HttpGet]
        public IActionResult EditItemQuantity(int orderItemId, string operation)
        {
            OrderItem orderItem = _orderItemService.GetOrderItemById(orderItemId);

            try
            {
                _orderItemService.EditItemQuantity(orderItem, operation);                
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An unexpected error occurred while editing the item quantity, please try again";                
            }
            return RedirectToAction("TakeOrder", new { orderNumber = orderItem.OrderNumber });
        }


        [HttpGet]
        public IActionResult DeleteSingleItem(int orderItemId)
        {
            OrderItem orderItem = _orderItemService.GetOrderItemById(orderItemId);

            try
            {
                _orderItemService.DeleteSingleItem(orderItem);               
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An unexpected error occurred while deleting the item, please try again";                
            }
            return RedirectToAction("TakeOrder", new { orderNumber = orderItem.OrderNumber });
        }


        [HttpPost]
        public IActionResult EditItemNote(int orderItemId, string note)
        {
            OrderItem orderItem = _orderItemService.GetOrderItemById(orderItemId);
            orderItem.Note = note;

            try
            {
                _orderItemService.EditItemNote(orderItem);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An unexpected error occurred while editing the item note, please try again";                
            }

            return RedirectToAction("TakeOrder", new { orderNumber = orderItem.OrderNumber });
        }

    }
}
