using Azure;
using Chapeau_Project_1._4.Models;
using Microsoft.AspNetCore.Mvc;

namespace Chapeau_Project_1._4.Services.OrderItems
{
    public interface IOrderItemService
    {
        void AddOrderItem(OrderItem orderItem);
        bool CheckDuplicateItems(OrderItem orderItem);

        public OrderItem GetOrderItemById(int orderItemId);

        List<OrderItem> DisplayOrderItems();
        List<OrderItem> DisplayItemsPerOrder(Chapeau_Project_1._4.Models.Order order);

        List<OrderItem> GetByOrderNumber(int orderNumber);

        void UpdateAllItemsStatus(Chapeau_Project_1._4.Models.Order order);

        void ReduceItemStock(OrderItem orderItem);

        void EditItemQuantity(OrderItem orderItem, string operation);
        void DeleteSingleItem(OrderItem orderItem);

        void EditItemNote(OrderItem orderItem);
    }
}
