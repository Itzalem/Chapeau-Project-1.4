using Chapeau_Project_1._4.Repositories.DrinkRepo;
using Chapeau_Project_1._4.Repositories.OrderItemRepo;
using Chapeau_Project_1._4.Repositories.OrderRepo;
using Chapeau_Project_1._4.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Chapeau_Project_1._4.ViewComponents.Drink
{
    public class DrinkViewComponent: ViewComponent
    {
        private readonly IDrinkRepository _drinkRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        public DrinkViewComponent(
            IDrinkRepository drinkRepository,
            IOrderRepository orderRepository,
            IOrderItemRepository orderItemRepository
            )
        {
            _drinkRepository = drinkRepository;
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
        }

        public IViewComponentResult Invoke()
        {
            var drinks = new List<string>();//_drinkRepository.GetDrinkOrders();
            return View(drinks);
        }
    }
}
