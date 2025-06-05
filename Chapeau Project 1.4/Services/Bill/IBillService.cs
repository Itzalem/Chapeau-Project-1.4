using Chapeau_Project_1._4.Models;

namespace Chapeau_Project_1._4.Services.Bill
{
    public interface IBillService
    {
        Chapeau_Project_1._4.Models.Bill CreateBill(Payment payment);
    }
}
