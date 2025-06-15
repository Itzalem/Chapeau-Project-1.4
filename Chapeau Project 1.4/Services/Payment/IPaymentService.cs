using Chapeau_Project_1._4.Models;

namespace Chapeau_Project_1._4.Services.Payment
{
    public interface IPaymentService
    {
        void CreateBill(Models.Payment payment);
        void CreatePayment(Models.Payment payment);
		Bill GetBill(Models.Payment payment);
        void UpdateOrderStatus(Models.Payment payment);
        void UpdateTableStatus(Models.Payment payment);
        Models.Payment SplitAmountsEqual(Models.Payment payment, int totalPay);
        decimal UpdatePayed(Models.Payment payment, decimal alreadyPayed);
	}
}
