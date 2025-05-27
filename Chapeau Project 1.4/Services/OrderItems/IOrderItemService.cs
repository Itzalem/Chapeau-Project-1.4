using Chapeau_Project_1._4.Models;

namespace Chapeau_Project_1._4.Services.OrderItems
{
    public interface IOrderItemService
    {
        void AddOrderItem(OrderItem orderItem);

        List<OrderItem> GetByOrderNumber(int orderNumber);
    }
}
