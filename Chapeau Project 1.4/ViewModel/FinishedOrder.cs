using Chapeau_Project_1._4.Models;

namespace Chapeau_Project_1._4.ViewModel
{
    public class FinishedOrder
    {
        public int OrderNumber { get; set; }
        public int TableNumber { get; set; }
        public DateTime OrderTime { get; set; }
        public int WaitingTime { get; set; }  // in minutes
        public EOrderStatus Status { get; set; }
        public List<FinishedOrderItem> finishedOrders { get; set; } = new List<FinishedOrderItem>(); 
    }

    public class FinishedOrderItem
    {
        public int OrderItemId { get; set; }
        //strings are naturally nullable, the green line under it tells us that it might not assign a value, either make it nullable with a ? or give it a value in the constructor 

        public FinishedOrderMenuItem FinishedOrderMenuItem { get; set; }
        public int Quantity { get; set; }
        public string Note { get; set; }
        public EItemStatus ItemStatus { get; set; }
    }

    public class FinishedOrderMenuItem
    {
        public string OrderItemName { get; set; }
        public string MenuItemCategoryStatus { get; set; }
        public string OrderItemCategory { get; set; }
    }
}
