using Chapeau_Project_1._4.Models;

namespace Chapeau_Project_1._4.Services.Order
{
    public interface IOrderService
    {
        int AddNewOrder(int tableNumber);

        Chapeau_Project_1._4.Models.Order? GetOrderByTable(int? table);

        void UpdateOrderStatus(EOrderStatus status, int orderNumber);

        Chapeau_Project_1._4.Models.Order? GetOrderByNumber(int orderNumber);

        void CancelUnsentOrder(Chapeau_Project_1._4.Models.Order order);

        List<OrderItem> GetOrderItems(int orderNumber);
    }
}
