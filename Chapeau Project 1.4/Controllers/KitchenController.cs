using Chapeau_Project_1._4.Models;
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
        public KitchenController(IOrderRepository orderRepository , IOrderItemRepository orderItemRepository)
        {
            _orderRepository = orderRepository; 
            _orderItemRepository = orderItemRepository;
        }
       
        public IActionResult Index()
        {
            var orders = _orderRepository.DiplayOrder();
            var orderItems = _orderItemRepository.DisplayOrderItem();



            var OrderViewModelResult = orders.Select(x => new RunningOrder
            {
                OrderNumber = x.OrderNumber,
                OrderTime = x.OrderTime,
                TableNumber = x.TableNumber,
                WaitingTime = (int)(DateTime.Now - x.OrderTime).TotalMinutes, 
                runningOrders = orderItems.Where(p => p.OrderNumber == x.OrderNumber).Select(o => new RunningOrderItem
                {
                    ItemStatus = o.ItemStatus,
                    OrderItemName = o.OrderItemId.ToString(),
                    Note = o.Note,
                    OrderItemId = o.OrderItemId,
                    Quantity = o.Quantity,
                }).ToList()
            }).ToList();


            return View("RunningOrder" , OrderViewModelResult);
        }
    }
}
