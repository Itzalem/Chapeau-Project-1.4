using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.Repositories.PaymentRepo;

namespace Chapeau_Project_1._4.Services.Payment
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository billRepository)
        {
			_paymentRepository = billRepository;
        }

        public void CreateBill(Models.Payment payment)
        {
            _paymentRepository.CreateBill(payment);
        }

        public void CreatePayment(Models.Payment payment)
        {
            //feedback cannot be null
            if (payment.Feedback == null)
            {
				payment.Feedback = "no feedback";
			}
            _paymentRepository.CreatePayment(payment);
        }

        public Bill GetBill(Models.Payment payment)
        {
            return _paymentRepository.GetBill(payment);
        }
        
        public void UpdateOrderStatus(Models.Payment payment)
        {
            _paymentRepository.UpdateOrderStatus(payment);
        }

        public void UpdateTableStatus(Models.Payment payment)
        {
            _paymentRepository.UpdateTableStatus(payment);
        }

        public Models.Payment SplitAmountsEqual(Models.Payment payment, int totalPay)
        {
            //at least 1 payment
            if (totalPay < 1)
                totalPay = 1;

            //split the Total and VAT between the customres
            payment.SplitTotal = payment.Total / totalPay;
            payment.SplitVAT = payment.VAT / totalPay;

            return payment;
        }

        public decimal UpdatePayed(Models.Payment payment, decimal alreadyPayed)
        {
            //update the amount payed up until this moment
            alreadyPayed += payment.AmountPayed;
			//get back extra money if someone overpayed
			if (alreadyPayed > payment.Total)
                payment.AmountPayed = payment.Total - (alreadyPayed - payment.AmountPayed);

			return alreadyPayed;
        }
	}
}
