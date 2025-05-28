using Chapeau_Project_1._4.Models;

namespace Chapeau_Project_1._4.ViewModel
{
    public class FinishOrder
    {
        public Payment Payment { get; set; }
        public Order Order { get; set; }
        public RestaurantTable Table { get; set; }
    }
}
