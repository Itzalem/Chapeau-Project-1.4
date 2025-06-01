
using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.Services.Order;
using Chapeau_Project_1._4.Services.RestaurantTableService;
using Microsoft.AspNetCore.Mvc;

namespace Chapeau_Project_1._4.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IRestaurantTableService _tableService;

        public PaymentController(IOrderService orderService, IRestaurantTableService tableService)
        {
            _orderService = orderService;
            _tableService = tableService;
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

        [HttpGet]
        public IActionResult PreparePay()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult PreparePay(Payment payment)
        //{
        //    Order? order = _orderService.GetOrderByTable(payment.Bill.Table.TableNumber);
        //    RestaurantTable? restaurantTable = _tableService.GetTableByNumber(payment.Bill.Table.TableNumber);
        //    Bill? bill = 
        //}
    }
}
