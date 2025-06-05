
using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.Services.OrderItems;
using Chapeau_Project_1._4.Services.Order;
using Chapeau_Project_1._4.Services.RestaurantTableService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient.DataClassification;
using System.Reflection;
using Chapeau_Project_1._4.Services.Bill;

namespace Chapeau_Project_1._4.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IOrderItemService _orderItemService;
        private readonly IRestaurantTableService _tableService;
        private readonly IBillService _billService;

        public PaymentController(IOrderService orderService, IRestaurantTableService tableService, IOrderItemService orderItemService, IBillService billService)
        {
            _orderService = orderService;
            _tableService = tableService;
            _orderItemService = orderItemService;
            _billService = billService;
        }

        [HttpGet]
        public IActionResult DisplayOrder(int? table)
        {
            Order? order = _orderService.GetOrderByTable(table);
            order.OrderItems = _orderItemService.DisplayItemsPerOrder(order);

            if (order == null)
            {
                return RedirectToAction("Overview", "RestaurantTable");
            }

            return View(order);
        }

        [HttpGet]
        public IActionResult PreparePay(Order order)
        {
            Bill bill = new Bill(order, _tableService.GetTableByNumber(order.TableNumber));

            Payment payment = new Payment(bill, order.Total);
            return View(payment);
        }

        [HttpPost]
        public IActionResult PreparePay(Payment payment)
        {
            _billService.CreateBill(payment);
            return RedirectToAction("RestaurantTable", "Overview");
        }
    }
}
