using Chapeau_Project_1._4.Models;

namespace Chapeau_Project_1._4.Services.Order
{
    public interface IOrderService
    {
        int AddNewOrder(int tableNumber);

        Chapeau_Project_1._4.Models.Order? GetOrderByTable(int? table);

    }
}
