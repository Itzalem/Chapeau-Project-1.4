using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.Repositories.MenuRepo;
using Chapeau_Project_1._4.Repositories.OrderRepo;

namespace Chapeau_Project_1._4.Services.Order
{
    public class OrderService : IOrderService
    {
        private IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public int AddNewOrder(int tableNumber)
        {
            return _orderRepository.AddNewOrder(tableNumber);
        }

        public Chapeau_Project_1._4.Models.Order? GetOrderByTable(int? table)
        {
          return _orderRepository.GetOrderByTable(table);
        }

        public void UpdateOrderStatus(EOrderStatus updatedItemStatus, int orderNumber)
        {
            _orderRepository.UpdateOrderStatus(updatedItemStatus, orderNumber);
        }


        public Chapeau_Project_1._4.Models.Order? GetOrderByNumber(int orderNumber)
        {
            return _orderRepository.GetOrderByNumber(orderNumber);
        }

        public void CancelUnsentOrder(Chapeau_Project_1._4.Models.Order order)
        {
            _orderRepository.CancelUnsentOrder(order);
        }
        public List<OrderItem> GetOrderItems(int orderNumber)
        {
            return _orderRepository.GetOrderItemsByOrderNumber(orderNumber);
        }
    }
}
