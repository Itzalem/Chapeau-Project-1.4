using Chapeau_Project_1._4.Models;

namespace Chapeau_Project_1._4.ViewModel
{
    public class RunningOrderItem
    {
        public int OrderItemId { get; set; }
        //strings are naturally nullable, the green line under it tells us that it might not assign a value, either make it nullable with a ? or give it a value in the constructor 


        public RunnigOrderMenuCategory RunningOrderItemCategory { get; set; }
        public int Quantity { get; set; }
        public string Note { get; set; }
        public EItemStatus ItemStatus { get; set; }

    }
}
