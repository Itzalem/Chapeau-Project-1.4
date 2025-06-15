using Chapeau_Project_1._4.Models;

namespace Chapeau_Project_1._4.ViewModel
{
	public class SplitBill
	{
		public Payment Payment { get; set; }
		public int TotalPay { get; set; }
		public int CurrentPay {  get; set; }
		public decimal AlreadyPayed { get; set; }

        public SplitBill(Payment payment, int totalPay, int currentPay)
        {
            Payment = payment;
			TotalPay = totalPay;
			CurrentPay = currentPay;
        }

		public SplitBill(Payment payment, decimal alreadyPayed)
		{
			Payment = payment;
			AlreadyPayed = alreadyPayed;
		}
	}
}
