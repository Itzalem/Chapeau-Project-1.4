using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.Services.Menu;
using Chapeau_Project_1._4.Services.Orders;
using Microsoft.AspNetCore.Mvc;

namespace Chapeau_Project_1._4.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public IActionResult TakeOrder(/*int tableNumber*/)
        {
            //_orderService.AddNewOrder(tableNumber);
            

            return View();
        }

        [HttpPost]
        public IActionResult AddItemsToOrder(MenuItem menuItem)
        {


            return RedirectToAction("DisplayMenu", "Menu");
        }

    
}
}
