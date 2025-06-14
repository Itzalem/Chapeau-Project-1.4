using Chapeau_Project_1._4.Models;

namespace Chapeau_Project_1._4.ViewModel
{
	public class SplitBill
	{
		public Payment Payment { get; set; }
		public int Payments { get; set; }
		public int PaymentsLeft
		{
			get
			{
				return Payments;
			}
			set { } 
		}
		public decimal AmountLeft 
		{ 
			get
			{
				return Payment.Bill.Order.Total;
			}
			set { }
		}
		public decimal VAT
		{ 
			get
			{
				return (Payment.VAT / Payments);
			} 
			set { }
		}

		public SplitBill(Payment payment, int payments)
		{
			Payment = payment;
			Payments = payments;
		}
	}
}
