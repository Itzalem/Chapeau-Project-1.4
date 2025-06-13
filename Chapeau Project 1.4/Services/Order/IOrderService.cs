using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.ViewModel;

namespace Chapeau_Project_1._4.Services.Order
{
    public interface IOrderService
    {
        int AddNewOrder(int tableNumber);

        Chapeau_Project_1._4.Models.Order? GetOrderByTable(int? table);

        void UpdateOrderStatus(EOrderStatus status, int orderNumber);

        void CancelUnsentOrder(Chapeau_Project_1._4.Models.Order order);

        List<OrderItem> GetOrderItems(int orderNumber);

        //Lukas
        List<OrderItem> GetItemsForServing(int orderNumber);
        void ServeFoodItems(int orderNumber);
        void ServeDrinkItems(int orderNumber);


        List<Chapeau_Project_1._4.Models.Order> DisplayOrder(string tabName = "RunningOrders");
        object GetOrderMunuItemName(int OrderNumber);
        List<Chapeau_Project_1._4.Models.Order> GetFinishedOrders();
        Chapeau_Project_1._4.Models.Order? GetOrderByNumber(int orderNumber);
        List<RunningOrderWithItemsViewModel> GetOrdersWithItems(string tabName = "RunningOrders");
    }
}
