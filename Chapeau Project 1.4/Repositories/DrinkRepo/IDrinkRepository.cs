using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.ViewModel;

namespace Chapeau_Project_1._4.Repositories.DrinkRepo
{
    public interface IDrinkRepository
    {
        Drink GetDrinks(int? drinkId);
        List<RunningOrder> GetDrinkOrders();
        List<Drink> GetFinishedDrinksOrder();

    }
}
