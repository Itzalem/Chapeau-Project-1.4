using Chapeau_Project_1._4.Models;

namespace Chapeau_Project_1._4.ViewModel
{

    public class RunningOrderItem
    {
        public int OrderItemId { get; set; }
        public string OrderItemName { get; set; }
        public int Quantity { get; set; }
        public string Note { get; set; }
        public EItemStatus ItemStatus { get; set; }

    }
    public class RunningOrder
    {
        public int OrderNumber { get; set; }
        public int TableNumber { get; set; }
        public DateTime OrderTime { get; set; }
        public int WaitingTime { get; set; }  // in minutes
        public List<RunningOrderItem> runningOrders { get; set; } = new List<RunningOrderItem>();   
        public RunningOrder()
        {

        }

        public RunningOrder(int orderNumber, int tableNumber, DateTime orderTime, int waitingTime)
        {
            OrderNumber = orderNumber;
            TableNumber = tableNumber;
            OrderTime = orderTime;
            WaitingTime = waitingTime;
        }   
    }
}
