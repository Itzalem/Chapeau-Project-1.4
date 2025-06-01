using Chapeau_Project_1._4.Models;

namespace Chapeau_Project_1._4.ViewModel
{
    public class TableOrderViewModel
    {
        public int TableNumber { get; set; }
        public bool IsOccupied { get; set; }
        public int AmountOfGuests { get; set; }

        public string FoodOrderStatus { get; set; } = "None";
        public string DrinkOrderStatus { get; set; } = "None";
    }
}
