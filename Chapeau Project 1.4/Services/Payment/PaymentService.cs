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
            if (totalPay == 0)
                totalPay = 1;

            payment.SplitTotal = payment.Total / totalPay;
            payment.SplitVAT = payment.VAT / totalPay;

            return payment;
        }
	}
}
