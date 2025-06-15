
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
				//get the order with the tableNumber
				Order? order = _orderService.GetOrderByTable(table);
				
				//if there is no order, send an error message
				if (order == null)
				{
                    TempData["NoOrderMessage"] = "This Table Does Not Have An Order";
                    return RedirectToAction("Details", "RestaurantTable", new { id = table });
                }

				//fill the order with the orderItems
				order.OrderItems = _orderItemService.DisplayItemsPerOrder(order);

				return View(order);
			}
			catch
			{
				//something went wrong, send an error message
                TempData["SomethingWrongMessage"] = "Order Could Not Be Displayed";
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
			//get the bill by tableNumber
			Bill bill = GetBill(table);
			return new Payment(bill, bill.Order.Total);
		}

		private Payment InsertBillAndPayment(Payment payment, int table)
		{
			//enter the bill, order and table in the payment
			payment.Bill = GetBill(table);

			//create the bill in the database
			_paymentService.CreateBill(payment);

			//create the payment in the database. use the new Bill, with BillId
			payment.Bill = _paymentService.GetBill(payment);
			_paymentService.CreatePayment(payment);

			return payment;
		}

        [HttpGet]
        public IActionResult PreparePay(int? table)
        {
			try
			{
                //get the payment by tableNumber
                Payment payment = GetPayment(table);

                return View(payment);
            }
			catch
			{
                //something went wrong, send an error message
                TempData["PaymentWrongMessage"] = "Payment Could Not Be Set Up";
                return RedirectToAction("DisplayOrder", "Payment", new { table = table });
            }
		}

        [HttpPost]
        public IActionResult PreparePay(Payment payment, int table)
        {
			try
			{
				//insert the bill and payment in the database
				payment = InsertBillAndPayment(payment, table);

				//update the orderstatus to paid and table occupation to free
				_paymentService.UpdateOrderStatus(payment);
				_paymentService.UpdateTableStatus(payment);

				TempData["PaySuccessMessage"] = "Payment Finished Successfully";

				return RedirectToAction("Overview", "RestaurantTable");
			}
			catch
			{
				//something went wrong, send an error message
				TempData["PaymentWrongMessage"] = "Payment Could Not Be Submitted";
				return RedirectToAction("DisplayOrder", "Payment", new { table = table });
			}
        }

        [HttpGet]
        public IActionResult SplitAmount(int? table)
        {
            return View(GetPayment(table));
        }

        [HttpPost]
        public IActionResult SplitAmount(int totalPay, int? table)
        {
			return RedirectToAction("SplitEqualPay", "Payment", new { totalPay = totalPay, table = table, currentPay = 1 });
        }

		[HttpGet]
		public IActionResult SplitEqualPay(int? table, int totalPay, int currentPay)
		{
			try
			{
				Payment payment = GetPayment(table);
				payment = _paymentService.SplitAmountsEqual(payment, totalPay);

				SplitBill splitBill = new SplitBill(payment, totalPay, currentPay);

				return View(splitBill);
			}
			catch
			{
				//something went wrong, send an error message
				TempData["PaymentWrongMessage"] = "Payment Could Not Be Set Up";
				return RedirectToAction("DisplayOrder", "Payment", new { table = table });
			}
		}

		[HttpPost]
		public IActionResult SplitEqualPay(Payment payment, int table, int totalPay, int currentPay)
		{
			try
			{
				//insert the bill and payment in the database
				payment = InsertBillAndPayment(payment, table);

				if (currentPay < totalPay)
				{
					currentPay++;
					TempData["PaySuccessMessage"] = "Payment Went Through Successfully";
					return RedirectToAction("SplitEqualPay", new { table = payment.Bill.Table.TableNumber, totalPay = totalPay, currentPay = currentPay });
				}
				else
				{
					//update the orderstatus to paid and table occupation to free
					_paymentService.UpdateOrderStatus(payment);
					_paymentService.UpdateTableStatus(payment);

					TempData["PaySuccessMessage"] = "Payment Finished Successfully";

					return RedirectToAction("Overview", "RestaurantTable");
				}
			}
			catch
			{
				//something went wrong, send an error message
				TempData["PaymentWrongMessage"] = "Payment Could Not Be Submitted";
				return RedirectToAction("DisplayOrder", "Payment", new { table = table });
			}
		}

		[HttpGet]
		public IActionResult SplitChooseAmount(int? table, decimal alreadyPayed)
		{
			try
			{
				Payment payment = GetPayment(table);

				SplitBill splitBill = new SplitBill(payment, alreadyPayed);

				return View(splitBill);
			}
			catch
			{
				//something went wrong, send an error message
				TempData["PaymentWrongMessage"] = "Payment Could Not Be Set Up";
				return RedirectToAction("DisplayOrder", "Payment", new { table = table });
			}
		}

		private Payment TemporaryTotal(Payment payment, int table)
		{
			//set up a temporary total to insert the split amount in the database
			decimal tempTotal = payment.Total;
			payment.Total = payment.AmountPayed;
			
			//insert the bill and payment in the database
			payment = InsertBillAndPayment(payment, table);
			
			//set back total
			payment.Total = tempTotal;

			return payment;
		}

		[HttpPost]
		public IActionResult SplitChooseAmount(Payment payment, int table, decimal alreadyPayed)
		{
			try
			{
				payment = TemporaryTotal(payment, table);

				//update the total amount that was payed
				alreadyPayed = _paymentService.UpdatePayed(payment, alreadyPayed);

				if (alreadyPayed < payment.Total)
				{
					//go to the next payment
					TempData["PaySuccessMessage"] = "Payment Went Through Successfully";
					return RedirectToAction("SplitChooseAmount", new { table = payment.Bill.Table.TableNumber, alreadyPayed = alreadyPayed });
				}
				else
				{
					//update the orderstatus to paid and table occupation to free
					_paymentService.UpdateOrderStatus(payment);
					_paymentService.UpdateTableStatus(payment);

					if (alreadyPayed == payment.Total)
						TempData["PaySuccessMessage"] = "Payment Finished Successfully";
					else
						TempData["PaySuccessMessage"] = $"Payment Finished Successfully. The Extra €{alreadyPayed - payment.Total} Payed Has Been Deposited Back";

					return RedirectToAction("Overview", "RestaurantTable");
				}
			
			}
			catch
			{
				//something went wrong, send an error message
				TempData["PaymentWrongMessage"] = "Payment Could Not Be Submitted";
				return RedirectToAction("DisplayOrder", "Payment", new { table = table });
			}
		}
	}
}