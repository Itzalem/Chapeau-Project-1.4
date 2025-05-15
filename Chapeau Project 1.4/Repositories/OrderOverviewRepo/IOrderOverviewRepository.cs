using Chapeau_Project_1._4.Models;

namespace Chapeau_Project_1._4.Repositories.OrderOverviewRepo
{
    public interface IOrderOverviewRepository
    {
        List<OverviewItem> DisplayOrderDrinks();
        List<OverviewItem> DisplayOrderDishes();
    }
}
