namespace Chapeau_Project_1._4.Models
{
    public class Order
    {
        public int OrderNumber { get; set; }    
        public EOrderStatus Status { get; set; }  
        public DateTime OrderTime { get; set; } 
        public int TableNumber { get; set; }

        //the list of orderitems 
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();


        public Order()
        {

        }

        public Order(int orderNumber, EOrderStatus status, DateTime orderTime, int tableNumber)
        {
            OrderNumber = orderNumber;
            Status = status;
            OrderTime = orderTime;
            TableNumber = tableNumber;
        }

    }
}
