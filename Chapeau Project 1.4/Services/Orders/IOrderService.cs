using Chapeau_Project_1._4.Models;

namespace Chapeau_Project_1._4.Services.Orders
{
    public interface IOrderService
    {
        Chapeau_Project_1._4.Models.Order? GetOrderByTable(int? table);
    }
}
