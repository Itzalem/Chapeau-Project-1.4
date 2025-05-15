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

        public List<OverviewItem> DisplayOrderDrinks(int table)
        {
            return _orderOverviewRepository.DisplayOrderDrinks(table);
        }

        public List<OverviewItem> DisplayOrderDishes(int table)
        {
            return _orderOverviewRepository.DisplayOrderDishes(table);
        }
    }
}
