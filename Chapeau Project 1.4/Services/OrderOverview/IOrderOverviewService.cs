using Chapeau_Project_1._4.Models;

namespace Chapeau_Project_1._4.Services.OrderOverview
{
    public interface IOrderOverviewService
    {
        List<OverviewItem> DisplayOrderDrinks(int table);
        List<OverviewItem> DisplayOrderDishes(int table);
    }
}
