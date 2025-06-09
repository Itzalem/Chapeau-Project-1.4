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

        // calculate the total price of the order
        public decimal Total
        {
            get
            {
                decimal total = 0;
                foreach (OrderItem item in OrderItems)
                {
                    total += item.MenuItem.Price;
                }
                return total;
            }
        }

        //add food items -> loop over OrderItems and then check if its food (if so add it?)
        //add drink items -> loop over OrderItems and then check if its drink (if so add it?)

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
