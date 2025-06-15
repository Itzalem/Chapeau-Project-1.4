using Chapeau_Project_1._4.Repositories.OrderRepo;
using Chapeau_Project_1._4.Services.Order;
using Microsoft.AspNetCore.Mvc;

namespace Chapeau_Project_1._4.ViewComponents.Food
{
    public class FoodViewComponent : ViewComponent
    {
        private readonly IOrderService _ordersService;
        public FoodViewComponent(
            IOrderService ordersService
            )
        {
            _ordersService = ordersService;
        }

        public IViewComponentResult Invoke(string filterValue)
        {
            var orders = _ordersService.GetOrdersWithItems(false , filterValue);
            if (string.IsNullOrEmpty(filterValue))
            {
                filterValue = "RunningOrders";
            }
            orders.ForEach(x => x.ShowCourse = filterValue != "RunningOrders");
            return View(orders);
        }
    }
}
