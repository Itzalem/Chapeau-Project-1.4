using Chapeau_Project_1._4.Models;

namespace Chapeau_Project_1._4.Repositories.PaymentRepo
{
    public interface IPaymentRepository
    {
        void CreateBill(Payment payment);
		void CreatePayment(Payment payment);
        Bill GetBill(Payment payment);
        void UpdateOrderStatus(Payment payment);
		void UpdateTableStatus(Payment payment);
	}
}
