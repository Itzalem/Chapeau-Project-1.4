﻿using Chapeau_Project_1._4.Models;
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

            _orderItemService.AddOrderItem(orderItem); //add item to Db


            return RedirectToAction("TakeOrder", new { orderNumber = orderItem.OrderNumber });

        }



        [HttpPost]
        public IActionResult SendOrder(Order order)
        {
            order.OrderItems = _orderItemService.DisplayItemsPerOrder(order); //to load the list in the order object

            //i pass only the order number because I'm reusing a kitchen method that its already expecting a status
            _orderService.UpdateOrderStatus(EOrderStatus.pending, order.OrderNumber);

            //the stock its reduced before the itemstatus is changes to avoid already
            //sent items to reduce the stock again 
            
            _orderItemService.ReduceItemStock(order);
             

            _orderItemService.UpdateAllItemsStatus(order);

           
                    _orderItemService.CheckDuplicateItems(order);
               

            TempData["SuccessMessage"] = "Order Sent Successfully";

            return RedirectToAction("TakeOrder", new { orderNumber = order.OrderNumber });
        }


        [HttpPost]
        public IActionResult CancelOrder(Order order)
        {
            order.OrderItems = _orderItemService.DisplayItemsPerOrder(order);  //to load the list in the order object

            _orderService.CancelUnsentOrder(order);

            TempData["CancelMessage"] = "Order Canceled";

            return RedirectToAction("Overview", "RestaurantTable");

        }


        [HttpGet]
        public IActionResult EditItemQuantity(int orderItemId, string operation)
        {
            OrderItem orderItem = _orderItemService.GetOrderItemById(orderItemId);

            _orderItemService.EditItemQuantity(orderItem, operation);

            return RedirectToAction("TakeOrder", new { orderNumber = orderItem.OrderNumber });
        }

        [HttpGet]
        public IActionResult DeleteSingleItem(int orderItemId)
        {
            OrderItem orderItem = _orderItemService.GetOrderItemById(orderItemId);

            _orderItemService.DeleteSingleItem(orderItem);

            return RedirectToAction("TakeOrder", new { orderNumber = orderItem.OrderNumber });
        }

        [HttpPost]
        public IActionResult EditItemNote(int orderItemId, string note)
        {
            OrderItem orderItem = _orderItemService.GetOrderItemById(orderItemId);
            orderItem.Note = note;

            _orderItemService.EditItemNote(orderItem);

            return RedirectToAction("TakeOrder", new { orderNumber = orderItem.OrderNumber });
        }

    }
}
