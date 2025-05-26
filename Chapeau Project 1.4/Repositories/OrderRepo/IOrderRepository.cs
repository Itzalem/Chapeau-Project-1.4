using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.ViewModel;

namespace Chapeau_Project_1._4.Repositories.OrderRepo
{
    public interface IOrderRepository
    {
        List<Order> DisplayOrder();

        // Get a single order by its OrderNumber.
        Order? GetOrderById(int id);
        // Update all properties of an order.
        void Update(Order order);
        // Update only the Status of an order.
        void UpdateOrderStatus (EOrderStatus status, int id);
    }
}
