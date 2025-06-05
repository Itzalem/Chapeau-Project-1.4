using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.Repositories.BillRepo;

namespace Chapeau_Project_1._4.Services.Bill
{
    public class BillService : IBillService
    {
        private readonly IBillRepository _billRepository;

        public BillService(IBillRepository billRepository)
        {
            _billRepository = billRepository;
        }

        public Models.Bill CreateBill(Payment payment)
        {
            return _billRepository.CreateBill(payment);
        }
    }
}
