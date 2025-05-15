using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.Repositories.OrderOverviewRepo;

namespace Chapeau_Project_1._4.Services.OrderOverview
{
    public class OrderOverviewService : IOrderOverviewService
    {
        private IOrderOverviewRepository _orderOverviewRepository;

        public OrderOverviewService(IOrderOverviewRepository orderOverviewRepository)
        {
            _orderOverviewRepository = orderOverviewRepository;
        }

        public List<OverviewItem> DisplayOrderDrinks()
        {
            return _orderOverviewRepository.DisplayOrderDrinks();
        }

        public List<OverviewItem> DisplayOrderDishes()
        {
            return _orderOverviewRepository.DisplayOrderDishes();
        }
    }
}
