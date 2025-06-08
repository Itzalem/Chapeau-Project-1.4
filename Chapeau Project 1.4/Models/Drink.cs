namespace Chapeau_Project_1._4.Models
{
    public class Drink
    {
        public int DrinkId { get; set; }        
        public string DrinkName { get; set; } 
        public bool IsAlcoholic { get; set; }   
        public int MenuItemId { get; set; } 



        public Drink()
        {

        }


        public Drink(int drinkId, string drinkName, bool isAlcoholic, int menuItemId)
        {
            DrinkId = drinkId;
            DrinkName = drinkName;
            IsAlcoholic = isAlcoholic;
            MenuItemId = menuItemId;
        }
    }
}
