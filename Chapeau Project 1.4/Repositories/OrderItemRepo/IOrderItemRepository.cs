using Chapeau_Project_1._4.Models;

namespace Chapeau_Project_1._4.Repositories.OrderItemRepo
{
    public interface IOrderItemRepository
    {
        void AddOrderItem(OrderItem orderItem);


        //Returns all items belonging to a specific order
        List<OrderItem> GetByOrderNumber (int orderNumber);
        List<OrderItem> DisplayOrderItems(); 
        List<OrderItem> GetRunningItem(); 
        //Change the status for one specific order‐item
        void UpdateItemStatus(int orderItemId, EItemStatus newStatus);
        void UpdateCourseStatus(int orderNumber, string courseCategory, EItemStatus newStatus);
        List<OrderItem> GetFinishedItems(DateTime date);
    }
}
