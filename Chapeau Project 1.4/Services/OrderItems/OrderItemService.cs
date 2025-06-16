using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.Repositories.OrderItemRepo;
using Chapeau_Project_1._4.ViewModel;
using System.Diagnostics;

namespace Chapeau_Project_1._4.Services.OrderItems
{
    public class OrderItemService : IOrderItemService
    {
        private IOrderItemRepository _orderItemRepository;

        public OrderItemService(IOrderItemRepository orderItemRepository)
        {
            _orderItemRepository = orderItemRepository;
        }

        public void AddOrderItem(OrderItem orderItem) //M
        {
            if (!_orderItemRepository.CheckDuplicateItems(orderItem))
            {
                _orderItemRepository.InsertOrderItem(orderItem);
            }
        }

        public void CheckDuplicateItems(Chapeau_Project_1._4.Models.Order order) //M
        {
            foreach (OrderItem orderItem in order.OrderItems)
            {
                if (orderItem.ItemStatus == EItemStatus.pending)
                {
                    _orderItemRepository.CheckDuplicateItems(orderItem);
                }
            }
        }

        public List<OrderItem> DisplayItemsPerOrder(Chapeau_Project_1._4.Models.Order order) //M
        {
            return _orderItemRepository.DisplayItemsPerOrder(order);
        }

        public void ReduceItemStock(Chapeau_Project_1._4.Models.Order order) //M
        {
            foreach (OrderItem orderItem in order.OrderItems)
            {
                if (orderItem.ItemStatus == EItemStatus.onHold) //this is not the best way to do it, better to do it in the repo
                {
                    _orderItemRepository.ReduceItemStock(orderItem);
                }
            }

        }

        public void UpdateHoldItemsStatus(Chapeau_Project_1._4.Models.Order order) //M
        {
            _orderItemRepository.UpdateHoldItemsStatus(order);

        }

        public OrderItem GetOrderItemById(int orderItemId) //M
        {
            return _orderItemRepository.GetOrderItemById(orderItemId);
        }         

        public void EditItemQuantity(OrderItem orderItem, string operation) //M
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

        public void DeleteSingleItem(OrderItem orderItem) //M
        {
            _orderItemRepository.DeleteSingleItem(orderItem);
        }
        public void EditItemNote(OrderItem orderItem) //M
        {
            _orderItemRepository.EditItemNote(orderItem);
        }

        public List<OrderItem> GetByOrderNumber(int orderNumber)
        {
            return _orderItemRepository.GetByOrderNumber(orderNumber);
        }     



        //Lukas
        public List<OrderItem> GetItemsForServing(int orderNumber)
        {
            try
            {
                // Returns all order items for a given order number that are in a state suitable for serving.
                return _orderItemRepository.GetOrderItemsForServing(orderNumber);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[GetItemsForServing Error] {ex.Message}");
                return new List<OrderItem>(); // Return an empty list if something goes wrong
            }
        }

        //Lukas
        public void ServeFoodItems(int orderNumber)
        {
            try
            {
                // Get all items for the given order that are candidates for serving
                List<OrderItem> items = _orderItemRepository.GetOrderItemsForServing(orderNumber);

                foreach (var item in items)
                {
                    // Check if the item is a food item (not a drink) AND is marked as ReadyToServe
                    if (!item.MenuItem.Card.Equals("Drinks", StringComparison.OrdinalIgnoreCase) &&
                        item.ItemStatus == EItemStatus.ReadyToServe)
                    {
                        // If both conditions match, update the item's status to Served
                        _orderItemRepository.UpdateItemStatus(item.OrderItemId, EItemStatus.Served);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ServeFoodItems Error] {ex.Message}");
            }
        }

        //Lukas
        public void ServeDrinkItems(int orderNumber)
        {
            try
            {
                // Get all items for the given order that are candidates for serving
                List<OrderItem> items = _orderItemRepository.GetOrderItemsForServing(orderNumber);

                foreach (var item in items)
                {
                    // Check if the item is a drink AND is marked as ReadyToServe
                    if (item.MenuItem.Card.Equals("Drinks", StringComparison.OrdinalIgnoreCase) &&
                        item.ItemStatus == EItemStatus.ReadyToServe)
                    {
                        // If both conditions match, update the item's status to Served
                        _orderItemRepository.UpdateItemStatus(item.OrderItemId, EItemStatus.Served);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ServeDrinkItems Error] {ex.Message}");
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

    }
}
