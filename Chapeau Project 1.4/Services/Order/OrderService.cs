using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.Repositories.OrderRepo;
using Chapeau_Project_1._4.Services.OrderItems;
using Chapeau_Project_1._4.Services.RestaurantTableService;
using Chapeau_Project_1._4.ViewModel;
using System.Diagnostics;

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
            //lukas - i added this
            int newOrderNumber = _orderRepository.AddNewOrder(tableNumber);
            _tableService.UpdateTableOccupancy(tableNumber, true);
            return newOrderNumber;
        }

        public Chapeau_Project_1._4.Models.Order? GetOrderByTable(int? table)
        {
            return _orderRepository.GetOrderByTable(table);
        }
        public Chapeau_Project_1._4.Models.Order? GetOrderByNumber(int orderNumber)
        {
            return _orderRepository.GetOrderByNumber(orderNumber);
        }

        public void CancelUnsentOrder(Chapeau_Project_1._4.Models.Order order) //M
        {
            _orderRepository.CancelUnsentOrder(order);
        }

        public List<OrderItem> GetOrderItems(int orderNumber)
        {
            return _orderRepository.GetOrderItemsByOrderNumber(orderNumber);
        }

        //Lukas
        public List<OrderItem> GetItemsForServing(int orderNumber)
        {
            try
            {
                // Calls the service layer to retrieve all items in the order 
                // that are ready or eligible to be served.
                return _orderItemService.GetItemsForServing(orderNumber);
            }
            catch (Exception ex)
            {
                // Log the error or handle it (for now we just output to debug)
                Debug.WriteLine($"[GetItemsForServing Error] {ex.Message}");
                return new List<OrderItem>(); // Return empty list on failure
            }
        }

        //Lukas
        public void ServeFoodItems(int orderNumber)
        {
            try
            {
                // Delegates the action to the service layer, which will:
                // - Find all food items (non-drinks) in the order
                // - Mark them as 'Served' if they are 'ReadyToServe'
                _orderItemService.ServeFoodItems(orderNumber);
            }
            catch (Exception ex)
            {
                // Log the error or handle it appropriately
                Debug.WriteLine($"[ServeFoodItems Error] {ex.Message}");
            }
        }

        //Lukas
        public void ServeDrinkItems(int orderNumber)
        {
            try
            {
                //same as ServeFoodItems but for Drinks
                _orderItemService.ServeDrinkItems(orderNumber);
            }
            catch (Exception ex)
            {
                // Log the error or handle it appropriately
                Debug.WriteLine($"[ServeDrinkItems Error] {ex.Message}");
            }
        }



        public List<Chapeau_Project_1._4.Models.Order> DisplayOrder()
        {
            return _orderRepository.DisplayOrder();    
        }

        //Mania 
        public object GetOrderMunuItemName(int OrderNumber)
        {
            return _orderRepository.GetOrderMunuItemName(OrderNumber);  
        }
        public void UpdateOrderStatus(EOrderStatus updatedItemStatus, int orderNumber)
        {
            _orderRepository.UpdateOrderStatus(updatedItemStatus, orderNumber);
        }

        public List<RunningOrderWithItemsViewModel> GetOrdersWithItems(bool IsDrink ,string tabName = "RunningOrders")
        {
            var queryResutl = _orderRepository.GetOrdersWithItems(IsDrink).ToList();
            List<RunningOrderWithItemsViewModel> orders = new();

            switch (tabName)
            {
                case "RunningOrders":
                    orders = queryResutl
                        .Where(x => x.Status != EOrderStatus.prepared).ToList();
                    break;
                case "FinishedOrders":
                    orders = queryResutl
                         .Where(x => x.Status == EOrderStatus.prepared).ToList();
                    break;
                default:
                    orders = queryResutl
                        .Where(x => x.Status != EOrderStatus.prepared).ToList();
                    break;

            }
            return orders;
        }
    }
}
