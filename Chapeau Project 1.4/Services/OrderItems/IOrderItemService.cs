using Chapeau_Project_1._4.Models;
using Microsoft.AspNetCore.Mvc;

namespace Chapeau_Project_1._4.Services.OrderItems
{
    public interface IOrderItemService
    {
        void AddOrderItem(OrderItem orderItem);

        List<OrderItem> DisplayOrderItems();
        List<OrderItem> DisplayItemsPerOrder(Chapeau_Project_1._4.Models.Order order);

        List<OrderItem> GetByOrderNumber(int orderNumber);

        void UpdateAllItemsStatus(Chapeau_Project_1._4.Models.Order order);

        void ReduceItemStock(OrderItem orderItem);

        void UpdateItemQuantity(OrderItem orderItem, int updatedQuantity);
        void DeleteSingleItem(OrderItem orderItem);

        void EditItemNote(OrderItem orderItem);
    }
}
