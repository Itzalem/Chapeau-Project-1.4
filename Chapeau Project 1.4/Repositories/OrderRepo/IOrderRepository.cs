using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.ViewModel;

namespace Chapeau_Project_1._4.Repositories.OrderRepo
{
    public interface IOrderRepository
    {
        int AddNewOrder(int tableNumber);
        void CancelUnsentOrder(Order order);

        // Get a single order by its OrderNumber.
        Order? GetOrderByNumber(int orderNumber);

        // Get a single order by its tableNumber
        Order? GetOrderByTable(int? table);

        // Update only the Status of an order.
        void UpdateOrderStatus(EOrderStatus status, int orderNumber);        
       
        // Update all properties of an order.
        void Update(Order order);        

        // دلیل استفاده از <آبجکت> این هست که ساختار کلاس مرتبط باهاش رو نداشتیم که اینستنس بگیریم ازش
        object GetOrderMunuItemName(int OrderNumber);

        List<Order> DisplayOrder();

        //for the overview - Lukas
        List<OrderItem> GetOrderItemsByOrderNumber(int orderNumber);

        List<RunningOrderWithItemsViewModel> GetOrdersWithItems(bool IsDrink);

    }
}
