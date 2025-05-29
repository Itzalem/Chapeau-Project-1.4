using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.Repositories.OrderItemRepo;

namespace Chapeau_Project_1._4.Services.OrderItems
{
    public class OrderItemService : IOrderItemService
    {
        private IOrderItemRepository _orderItemRepository;

        public OrderItemService(IOrderItemRepository orderItemRepository)
        {
            _orderItemRepository = orderItemRepository;
        }

        public void AddOrderItem(OrderItem orderItem)
        {
            _orderItemRepository.AddOrderItem(orderItem);
        }

        public List<OrderItem> DisplayOrderItems()
        {
            return _orderItemRepository.DisplayOrderItems();
        }

        public List<OrderItem> DisplayItemsPerOrder(Chapeau_Project_1._4.Models.Order order)
        {
            return _orderItemRepository.DisplayItemsPerOrder(order);
        }
        

        public List<OrderItem> GetByOrderNumber(int orderNumber)
        {
            return _orderItemRepository.GetByOrderNumber(orderNumber);
        }

        public void UpdateAllItemsStatus(EItemStatus itemStatus, Chapeau_Project_1._4.Models.Order order)
        {
            _orderItemRepository.UpdateAllItemsStatus(itemStatus, order);
        }
    }
}
