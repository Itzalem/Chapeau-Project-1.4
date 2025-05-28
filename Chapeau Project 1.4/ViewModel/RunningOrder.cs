using Chapeau_Project_1._4.Models;

namespace Chapeau_Project_1._4.ViewModel
{
    public class RunningOrder
    {
        public int OrderNumber { get; set; }
        public int TableNumber { get; set; }
        public DateTime OrderTime { get; set; }
        public int WaitingTime { get; set; }  // in minutes
        public List<RunningOrderItem> runningOrders { get; set; } = new List<RunningOrderItem>();   
        // Constructor is domain based, we don't need it in the viewmodel (UI) 
    }
}
