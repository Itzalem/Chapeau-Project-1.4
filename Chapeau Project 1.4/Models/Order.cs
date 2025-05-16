namespace Chapeau_Project_1._4.Models
{
    public class Order
    {
        public int OrderNumber { get; set; }    
        public string Status { get; set; }  
        public DateTime OrderTime { get; set; } 
        public int TableNumber { get; set; }    


        public Order()
        {

        }

        public Order (int orderNumber, string status, DateTime orderTime, int tableNumber)
        {
            OrderNumber = orderNumber;
            Status = status;
            OrderTime = orderTime;
            TableNumber = tableNumber;
        }   
    }
}
