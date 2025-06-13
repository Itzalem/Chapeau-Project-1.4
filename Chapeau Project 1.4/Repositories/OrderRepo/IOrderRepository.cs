using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.ViewModel;

namespace Chapeau_Project_1._4.Repositories.OrderRepo
{
    public interface IOrderRepository
    {
        int AddNewOrder(int tableNumber);
        List<Order> DisplayOrder();

        void CancelUnsentOrder(Order order);

        // Get a single order by its OrderNumber.
        Order? GetOrderByNumber(int orderNumber);
        // Get a single order by its tableNumber
        Order? GetOrderByTable(int? table);
        // Update all properties of an order.
        void Update(Order order);
        // Update only the Status of an order.
        void UpdateOrderStatus(EOrderStatus status, int orderNumber);

        object GetOrderMunuItemName(int OrderNumber);


        //for the overview - Lukas
        List<OrderItem> GetOrderItemsByOrderNumber(int orderNumber);

        List<Order> GetFinishedOrders();

        List<RunningOrderWithItemsViewModel> GetOrdersWithItems();

    }
}
