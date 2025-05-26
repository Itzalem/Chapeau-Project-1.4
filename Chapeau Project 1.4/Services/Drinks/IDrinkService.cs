using Chapeau_Project_1._4.Models;

namespace Chapeau_Project_1._4.Services.Drinks
{
    public interface IDrinkService
    {
        Drink GetDrinks(int? drinkId);
    }
}
