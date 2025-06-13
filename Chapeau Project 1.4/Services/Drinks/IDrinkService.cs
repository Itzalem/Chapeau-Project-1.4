using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.ViewModel;

namespace Chapeau_Project_1._4.Services.Drinks
{
    public interface IDrinkService
    {
        Drink GetDrinks(int? drinkId);
        //List<RunningOrder> GetDrinkOrders();
        List<Drink> GetFinishedDrinksOrder();
    }
}
