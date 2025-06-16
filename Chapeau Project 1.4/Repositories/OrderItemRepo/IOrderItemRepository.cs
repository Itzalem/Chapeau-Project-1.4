using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.ViewModel;

namespace Chapeau_Project_1._4.Repositories.OrderItemRepo
{
    public interface IOrderItemRepository
    {
        List<OrderItem> DisplayItemsPerOrder(Chapeau_Project_1._4.Models.Order order);

        void InsertOrderItem(OrderItem orderItem);

        bool CheckDuplicateItems(OrderItem orderItem);

        void ReduceItemStock(OrderItem orderItem);

        void UpdateHoldItemsStatus(Order order);

        public OrderItem GetOrderItemById(int orderItemId);

        public void EditItemQuantity(OrderItem orderItem);
        void DeleteSingleItem(OrderItem orderItem);
        void EditItemNote(OrderItem orderItem);


        //Returns all items belonging to a specific order
        List<OrderItem> GetByOrderNumber(int orderNumber);
        List<OrderItem> DisplayOrderItems();
        List<OrderItem> GetRunningItem();

        //Change the status for one specific order‐item
        void UpdateItemStatus(int orderItemId, EItemStatus newStatus);
        void UpdateCourseStatus(int orderNumber, string category, ECategoryStatus categoryCourseStatus);

        //Lukas
        List<OrderItem> GetOrderItemsForServing(int orderNumber);
    }
}
