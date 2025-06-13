using Chapeau_Project_1._4.Models;

namespace Chapeau_Project_1._4.ViewModel
{
    public class RunningOrderWithItemsViewModel
    {
        public int OrderNumber { get; set; }
        public int TableNumber { get; set; }
        public DateTime OrderTime { get; set; }
        public TimeSpan WaitingTime { get; set; }
        public DateTime? FinishOrderTime { get; set; }
        public EOrderStatus Status { get; set; }
        public List<RunningOrderItemWithMenuItemViewModel> runningOrderItems { get; set; } = new List<RunningOrderItemWithMenuItemViewModel>();
        // Constructor is domain based, we don't need it in the viewmodel (UI) 
    }
}
