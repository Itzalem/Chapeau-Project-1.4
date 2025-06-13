using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.Repositories.OrderItemRepo;
using Chapeau_Project_1._4.ViewModel;

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

        public bool CheckDuplicateItems(OrderItem orderItem)
        {
            return _orderItemRepository.CheckDuplicateItems(orderItem);
        }

        public OrderItem GetOrderItemById(int orderItemId)
        {
            return _orderItemRepository.GetOrderItemById(orderItemId);
        }

        public List<OrderItem> DisplayItemsPerOrder(Chapeau_Project_1._4.Models.Order order)
        {
            return _orderItemRepository.DisplayItemsPerOrder(order);
        }


        public List<OrderItem> GetByOrderNumber(int orderNumber)
        {
            return _orderItemRepository.GetByOrderNumber(orderNumber);
        }

        public void UpdateAllItemsStatus(Chapeau_Project_1._4.Models.Order order)
        {
            _orderItemRepository.UpdateAllItemsStatus(order);

        }

        public void ReduceItemStock(OrderItem orderItem)
        {
            _orderItemRepository.ReduceItemStock(orderItem);
        }


        public void EditItemQuantity(OrderItem orderItem, string operation)
        {
            if (operation == "increase")
            {
                orderItem.Quantity++;
                _orderItemRepository.EditItemQuantity(orderItem); ;
            }
            else if (operation == "decrease" && orderItem.Quantity > 1)
            {
                orderItem.Quantity--;
                _orderItemRepository.EditItemQuantity(orderItem); ;
            }
            else 
            {
                DeleteSingleItem(orderItem);
            }
        }

        public void DeleteSingleItem(OrderItem orderItem)
        {
            _orderItemRepository.DeleteSingleItem(orderItem);
        }

        public void EditItemNote(OrderItem orderItem)
        {
            _orderItemRepository.EditItemNote(orderItem);
        }


        //Lukas
        public List<OrderItem> GetItemsForServing(int orderNumber)
        {
            return _orderItemRepository.GetOrderItemsForServing(orderNumber);
        }


        //Lukas
        public void ServeFoodItems(int orderNumber)
        {
            List<OrderItem> items = _orderItemRepository.GetOrderItemsForServing(orderNumber);
            foreach (var item in items)
            {
                if (!item.MenuItem.Card.Equals("Drinks", StringComparison.OrdinalIgnoreCase) &&
                    item.ItemStatus == EItemStatus.ReadyToServe)
                {
                    _orderItemRepository.UpdateItemStatus(item.OrderItemId, EItemStatus.Served);
                }
            }
        }

        //Lukas
        public void ServeDrinkItems(int orderNumber)
        {
            List<OrderItem> items = _orderItemRepository.GetOrderItemsForServing(orderNumber);
            foreach (var item in items)
            {
                if (item.MenuItem.Card.Equals("Drinks", StringComparison.OrdinalIgnoreCase) &&
                    item.ItemStatus == EItemStatus.ReadyToServe)
                {
                    _orderItemRepository.UpdateItemStatus(item.OrderItemId, EItemStatus.Served);
                }
            }
        }


        public List<OrderItem> DisplayOrderItems()
        {
            return _orderItemRepository.DisplayOrderItems();
        }

        public List<OrderItem> GetRunningItem()
        {
            return _orderItemRepository.GetRunningItem();   
        }

        public void UpdateItemStatus(int orderItemId, EItemStatus newStatus)
        {
            _orderItemRepository.UpdateItemStatus(orderItemId, newStatus);  
        }

        public void UpdateCourseStatus(int orderNumber, string category, ECategoryStatus categoryCourseStatus)
        {
            _orderItemRepository.UpdateCourseStatus(orderNumber, category, categoryCourseStatus);    
        }

        public List<OrderItem> GetFinishedItems()
        {
            return _orderItemRepository.GetFinishedItems();
        }
    }
}
