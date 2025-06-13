using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.Repositories.DrinkRepo;
using Chapeau_Project_1._4.ViewModel;

namespace Chapeau_Project_1._4.Services.Drinks
{
    public class DrinkService:IDrinkService
    {
        private IDrinkRepository _drinkRepository;
        public DrinkService(IDrinkRepository drinkRepository)
        {
            _drinkRepository = drinkRepository;
        }

        public Drink GetDrinks(int? drinkId)
        {
            return _drinkRepository.GetDrinks(drinkId);     
        }

        //public List<RunningOrder> GetDrinkOrders()
        //{
        //    return _drinkRepository.GetDrinkOrders();
        //}

        public List<Drink> GetFinishedDrinksOrder()
        {
            return _drinkRepository.GetFinishedDrinksOrder();
        }
    }
}
