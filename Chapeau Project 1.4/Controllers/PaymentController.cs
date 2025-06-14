
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

		private Bill GetBill(int? table)
		{
			Order? order = _orderService.GetOrderByTable(table);
			order.OrderItems = _orderItemService.DisplayItemsPerOrder(order);

			return new Bill(order, _tableService.GetTableByNumber(table));
		}

        private Payment GetPayment(int? table)
        {
			Bill bill = GetBill(table);
			return new Payment(bill, bill.Order.Total);
		}

        [HttpGet]
        public IActionResult PreparePay(int? table)
        {
			Payment payment = GetPayment(table);

			return View(payment);
		}

        [HttpPost]
        public IActionResult PreparePay(Payment payment, int table)
        {
            //enter the bill, order and table in the payment
            payment.Bill = GetBill(table);

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
            return View(GetPayment(table));
        }

        [HttpPost]
        public IActionResult SplitAmount(int totalPay, int? table)
        {
            return RedirectToAction("SplitEqualPay", "Payment", new { totalPay = totalPay, table = table, currentPay = 1});
        }

		[HttpGet]
		public IActionResult SplitEqualPay(int? table, int totalPay, int currentPay)
		{
			Payment payment = GetPayment(table);
			payment = _paymentService.SplitAmountsEqual(payment, totalPay);

			SplitBill splitBill = new SplitBill(payment, totalPay, currentPay);

			return View(splitBill); 
		}

		[HttpPost]
		public IActionResult SplitEqualPay(Payment payment, int table, int totalPay, int currentPay)
		{
			//enter the bill, order and table in the payment
			payment.Bill = GetBill(table);

			//create the bill in the database
			_paymentService.CreateBill(payment);

			//create the payment in the database. use the new Bill, with BillId
			payment.Bill = _paymentService.GetBill(payment);
			_paymentService.CreatePayment(payment);

			if (currentPay < totalPay)
			{
				currentPay++;
				TempData["paySuccessMessage"] = "Payment Went Through Successfully";
				return RedirectToAction("SplitEqualPay", new { table = payment.Bill.Table.TableNumber, totalPay = totalPay, currentPay = currentPay });
			}
			else
			{
				//update the orderstatus to paid and table occupation to free
				_paymentService.UpdateOrderStatus(payment);
				_paymentService.UpdateTableStatus(payment);

				TempData["paySuccessMessage"] = "Payment Finished Successfully";

				return RedirectToAction("Overview", "RestaurantTable");
			}
		}


	}
}