
using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.Services.OrderItems;
using Chapeau_Project_1._4.Services.Order;
using Chapeau_Project_1._4.Services.RestaurantTableService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient.DataClassification;
using System.Reflection;
using Chapeau_Project_1._4.Services.Payment;
using Chapeau_Project_1._4.ViewModel;

namespace Chapeau_Project_1._4.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IOrderItemService _orderItemService;
        private readonly IRestaurantTableService _tableService;
        private readonly IPaymentService _paymentService;

        public PaymentController(IOrderService orderService, IRestaurantTableService tableService, IOrderItemService orderItemService, IPaymentService billService)
        {
            _orderService = orderService;
            _tableService = tableService;
            _orderItemService = orderItemService;
			_paymentService = billService;
        }

        [HttpGet]
        public IActionResult DisplayOrder(int? table)
        {
			try
			{
				Order? order = _orderService.GetOrderByTable(table);
				if (order == null)
					return RedirectToAction("Overview", "RestaurantTable");
				order.OrderItems = _orderItemService.DisplayItemsPerOrder(order);

				return View(order);
			}
			catch
			{
				return RedirectToAction("Overview", "RestaurantTable");
			}
        }

        private Payment GetPayment(int? table)
        {
			Order? order = _orderService.GetOrderByTable(table);
			order.OrderItems = _orderItemService.DisplayItemsPerOrder(order);

			Bill bill = new Bill(order, _tableService.GetTableByNumber(table));

			return new Payment(bill, order.Total);
		}

        [HttpGet]
        private IActionResult PreparePay(int? table)
        {
			Payment payment = GetPayment(table);

			return View(payment);
		}

        [HttpPost]
        public IActionResult PreparePay(Payment payment, int table)
        {
            //enter the bill, order and table in the payment
			Order? order = _orderService.GetOrderByTable(table);
			order.OrderItems = _orderItemService.DisplayItemsPerOrder(order);
			Bill bill = new Bill(order, _tableService.GetTableByNumber(table));
            payment.Bill = bill;

			//create the bill in the database
			_paymentService.CreateBill(payment);

            //create the payment in the database. use the new Bill, with BillId
            payment.Bill = _paymentService.GetBill(payment);
            _paymentService.CreatePayment(payment);

            //update the orderstatus to paid and table occupation to free
            _paymentService.UpdateOrderStatus(payment);
            _paymentService.UpdateTableStatus(payment);

            TempData["paySuccessMessage"] = "Payment Finished Successfully";

            return RedirectToAction("Overview", "RestaurantTable");
        }

        [HttpGet]
        public IActionResult SplitAmount(int? table)
        {
            return View(table);
        }

        [HttpPost]
        public IActionResult SplitAmount(int payments, int? table)
        {
            return RedirectToAction("SplitEqualPay", "Payment", new { payments = payments, table = table });
        }

        [HttpGet]
        public void SplitEqualPay(int payments, int? table)
        {
            Payment payment = GetPayment(table);
            SplitBill splitBill = new SplitBill(payment, payments);

            for (int i = 0; i < payments; i++)
            {
                GoToSplitEqual(splitBill);
            }
        }

		private IActionResult GoToSplitEqual(SplitBill splitBill)
        {
            return View(splitBill);
        }

	}
}