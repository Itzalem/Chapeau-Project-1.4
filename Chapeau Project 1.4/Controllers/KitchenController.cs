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

            // in order to fill in the order, I need to have OrderItemName and ORderItemCategory, 
            // a method must be created to fetch these two and give it to me and the outcome must be RunningOrderMenuCategory 
            // var x = _repositoy.getData();

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
                    RunningOrderItemCategory = new RunnigOrderMenuCategory
                    {
                        OrderItemName = o.OrderItemId.ToString(),
                        OrderItemCategory = ""
                    },
                    Note = o.Note,
                    OrderItemId = o.OrderItemId,
                    Quantity = o.Quantity,
                }).ToList()
            }).ToList();


            return View("RunningOrder" , OrderViewModelResult);
        }
    }
}
