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

            var OrderViewModelResult = orders.Select(x => new RunningOrder
            {
                OrderNumber = x.OrderNumber,
                OrderTime = x.OrderTime,
                TableNumber = x.TableNumber,
                Status = x.Status,
                WaitingTime = DateTime.Now - x.OrderTime,
                runningOrders = orderItems.Where(p => p.OrderNumber == x.OrderNumber).Select(o => new RunningOrderItem
                {
                    ItemStatus = o.ItemStatus,
                   
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
        public IActionResult UpdateItemStatus(int orderItemId , EItemStatus groupStatus)
        {
            
            // Call into your repo to update a single item
            _orderItemRepository.UpdateItemStatus(orderItemId, groupStatus);
            // Redirect back to the running-orders page so the view reloads
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateCourseStatus (int orderNumber, EItemStatus itemStatus)
        {
            _orderItemRepository.UpdateCourseStatus(orderNumber,  itemStatus);  
            return RedirectToAction("Index");
        }



        [HttpPost]
        public IActionResult UpdateOrderStatus(int orderNumber, EOrderStatus orderStatus)
        {
            _orderRepository.UpdateOrderStatus(orderStatus, orderNumber);
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult GetFinishedItems()
        {
            return View("FinishedOrder"); 
        }
    }
}
