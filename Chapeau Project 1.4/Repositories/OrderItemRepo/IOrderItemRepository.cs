using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.ViewModel;

namespace Chapeau_Project_1._4.Repositories.OrderItemRepo
{
    public interface IOrderItemRepository
    {
        
        bool CheckDuplicateItems(OrderItem orderItem);
        void InsertOrderItem(OrderItem orderItem);

        public OrderItem GetOrderItemById(int orderItemId);

        //Returns all items belonging to a specific order
        List<OrderItem> GetByOrderNumber(int orderNumber);
        List<OrderItem> DisplayOrderItems();
        List<OrderItem> DisplayItemsPerOrder(Chapeau_Project_1._4.Models.Order order);
        List<OrderItem> GetRunningItem();

        //Change the status for every order item in the list thats onHold
        void UpdateHoldItemsStatus(Order order);

        void ReduceItemStock(OrderItem orderItem);

        //Change the status for one specific order‐item
        void UpdateItemStatus(int orderItemId, EItemStatus newStatus);
        void UpdateCourseStatus(int orderNumber, string category, ECategoryStatus categoryCourseStatus);


        public void EditItemQuantity(OrderItem orderItem);
        void DeleteSingleItem(OrderItem orderItem);

        void EditItemNote(OrderItem orderItem);

        //Lukas
        List<OrderItem> GetOrderItemsForServing(int orderNumber);
    }
}
