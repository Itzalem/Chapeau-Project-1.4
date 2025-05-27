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

        //public void AddNewOrder(int tableNumber)
        //{
        // _orderRepository.CreateNewOrder(tableNumber);
        //}

        /*public Order? GetOrderByTable(int? table)
      {
          return _orderRepository.GetOrderByTable(table);
      }*/
    }
}
