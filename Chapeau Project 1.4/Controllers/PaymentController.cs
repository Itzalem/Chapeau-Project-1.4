using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.Services.Order;
using Microsoft.AspNetCore.Mvc;

namespace Chapeau_Project_1._4.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IOrderService _orderService;

        public PaymentController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public IActionResult DisplayOrder(int? table)
        {
            Order? order = _orderService.GetOrderByTable(table);

            if (order == null)
            {
                return RedirectToAction("Overview", "RestaurantTable");
            }

            return View(order);
        }
    }
}
