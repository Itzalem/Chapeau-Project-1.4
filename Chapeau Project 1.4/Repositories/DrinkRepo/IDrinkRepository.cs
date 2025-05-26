using Chapeau_Project_1._4.Models;

namespace Chapeau_Project_1._4.Repositories.DrinkRepo
{
    public interface IDrinkRepository
    {
        Drink GetDrinks(int? drinkId);
    }
}
