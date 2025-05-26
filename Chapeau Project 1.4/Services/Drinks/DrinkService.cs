using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.Repositories.DrinkRepo;

namespace Chapeau_Project_1._4.Services.Drinks
{
    public class DrinkService : IDrinkService
    {
        private IDrinkRepository _drinkRepository;

        public DrinkService(IDrinkRepository orderOverviewRepository)
        {
            _drinkRepository = orderOverviewRepository;
        }

        public Drink GetDrinks(int? drinkId)
        {
            return _drinkRepository.GetDrinks(drinkId);
        }
    }
}
