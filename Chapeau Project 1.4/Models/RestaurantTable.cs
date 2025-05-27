namespace Chapeau_Project_1._4.Models
{
    public class RestaurantTable
    {
        public int TableNumber { get; set; }
        public int AmountOfGuests { get; set; }
        public bool IsOccupied { get; set; }

        public RestaurantTable()
        {
            
        }
        public RestaurantTable(int tableNumber, int amountOfGuests, bool isOccupied)
        {
            TableNumber = tableNumber;
            AmountOfGuests = amountOfGuests;
            IsOccupied = isOccupied;
        }
    }
}
