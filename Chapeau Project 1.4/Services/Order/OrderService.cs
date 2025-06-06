using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.Repositories.MenuRepo;
using Chapeau_Project_1._4.Repositories.OrderRepo;
using Chapeau_Project_1._4.Repositories.OrderItemRepo;
using Chapeau_Project_1._4.Services.RestaurantTableService;
using Chapeau_Project_1._4.Services.OrderItems;

namespace Chapeau_Project_1._4.Services.Order
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IRestaurantTableService _tableService;
        private readonly IOrderItemService _orderItemService;

        public OrderService(
            IOrderRepository orderRepository,
            IRestaurantTableService tableService,
            IOrderItemService orderItemService)
        {
            _orderRepository = orderRepository;
            _tableService = tableService;
            _orderItemService = orderItemService;
        }

        public int AddNewOrder(int tableNumber)
        {
            //return _orderRepository.AddNewOrder(tableNumber);

            //lukas - i added this
            int newOrderNumber = _orderRepository.AddNewOrder(tableNumber);
            _tableService.UpdateTableOccupancy(tableNumber, true);
            return newOrderNumber;
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
