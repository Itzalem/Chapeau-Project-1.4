using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.ViewModel;

namespace Chapeau_Project_1._4.Services.Order
{
    public interface IOrderService
    {
        int AddNewOrder(int tableNumber);

        Chapeau_Project_1._4.Models.Order? GetOrderByTable(int? table);

        Chapeau_Project_1._4.Models.Order? GetOrderByNumber(int orderNumber);

        void UpdateOrderStatus(EOrderStatus status, int orderNumber);

        void CancelUnsentOrder(Chapeau_Project_1._4.Models.Order order);

        List<OrderItem> GetOrderItems(int orderNumber);

        //Lukas
        List<OrderItem> GetItemsForServing(int orderNumber);
        void ServeFoodItems(int orderNumber);
        void ServeDrinkItems(int orderNumber);


        List<Chapeau_Project_1._4.Models.Order> DisplayOrder();
        object GetOrderMunuItemName(int OrderNumber);
        List<RunningOrderWithItemsViewModel> GetOrdersWithItems(bool IsDrink,string tabName = "RunningOrders");
    }
}
