namespace Chapeau_Project_1._4.ViewModel
{
    public class RunningOrder
    {
        public int OrderNumber { get; set; }
        public int TableNumber { get; set; }
        public DateTime OrderTime { get; set; }
        public int WaitingTime { get; set; }  // in minutes

        
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
