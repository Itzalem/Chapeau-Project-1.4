using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.Repositories.MenuRepo;
using Chapeau_Project_1._4.Repositories.OrderItemRepo;
using Chapeau_Project_1._4.Repositories.OrderRepo;
using Chapeau_Project_1._4.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Chapeau_Project_1._4.Controllers
{
    public class KitchenController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IMenuRepository _menuRepository;
        
        public KitchenController(IOrderRepository orderRepository, IOrderItemRepository orderItemRepository, IMenuRepository menuRepository)
        {
            _orderRepository = orderRepository; 
            _orderItemRepository = orderItemRepository; 
            _menuRepository = menuRepository;
        }

        public IActionResult Index()
        {
            var orders = _orderRepository.DisplayOrder();
            var orderItems = _orderItemRepository.DisplayOrderItems(); 

            //in order to fill in the order, I need to have OrderItemName and OrderItemCategory,
            //a method must be created to fetch these two and give it to me and the outcome must be RunningOrderMenuCategory
            //var x = _repositoy.getData();

            var OrderViewModelResult = orders.Select(x => new RunningOrder
            {
                OrderNumber = x.OrderNumber,
                OrderTime = x.OrderTime,
                TableNumber = x.TableNumber,
                WaitingTime = (int)(DateTime.Now - x.OrderTime).TotalMinutes,
                runningOrders = orderItems.Where(p => p.OrderNumber == x.OrderNumber).Select(o => new RunningOrderItem
                {
                    ItemStatus = o.ItemStatus,
                   
                    // RunningOrderItemCategory = x.
                    RunnigOrderMenuItem = new RunnigOrderMenuItem
                    {
                        OrderItemName =o.MenuItem.MenuItemName,
                        OrderItemCategory = o.MenuItem.Category,
                    },
                    Note = o.Note,
                    OrderItemId = o.OrderItemId,
                    Quantity = o.Quantity,
                }).ToList()
            }).ToList();


            return View("RunningOrder", OrderViewModelResult);   
        }

        [HttpPost]
        public IActionResult UpdateItemStatus(int orderItemId , int orderNumber, EItemStatus itemStatus)
        {
            
            // Call into your repo to update a single item
            _orderItemRepository.UpdateItemStatus(orderItemId, itemStatus);
            // Redirect back to the running-orders page so the view reloads
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateCourseStatus (int orderNumber, string itemStatus)
        {
            _orderItemRepository.UpdateCourseStatus(orderNumber, (EItemStatus)Enum.Parse(typeof(EItemStatus) , itemStatus));  
            return RedirectToAction("Index");
        }
    }
}
