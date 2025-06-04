using Chapeau_Project_1._4.Models;

namespace Chapeau_Project_1._4.Repositories.DrinkRepo
{
    public interface IDrinkRepository
    {
        Drink GetDrinks(int? drinkId);
        List<Drink> GetDrinkOrders(); 
        List<Drink> GetFinishedDrinksOrder(); 

        // there are also the update status methods, however we already implemented them in the orderItem and order class 

    }
}
