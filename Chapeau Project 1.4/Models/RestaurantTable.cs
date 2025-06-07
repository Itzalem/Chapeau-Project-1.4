namespace Chapeau_Project_1._4.Models
{
    public class RestaurantTable
    {
        public int TableNumber { get; set; }
        public bool IsOccupied { get; set; }
        public bool WasManuallyFreed { get; set; }


        public RestaurantTable()
        {
            
        }
        public RestaurantTable(int tableNumber, bool isOccupied, bool wasManuallyFreed)
        {
            TableNumber = tableNumber;
            IsOccupied = isOccupied;
            WasManuallyFreed = wasManuallyFreed;
        }
    }
}
