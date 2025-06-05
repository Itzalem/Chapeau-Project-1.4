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
            set { }
        }

        public Order()
        {

        }

        public Order(int orderNumber, EOrderStatus status, DateTime orderTime, int tableNumber)
        {
            OrderNumber = orderNumber;
            Status = status;
            OrderTime = DateTime.Now;
            TableNumber = tableNumber;
        }

        /*public Order(int orderNumber, EOrderStatus status, DateTime orderTime, int tableNumber, List<OrderItem> orderItems)
        {
            OrderNumber = orderNumber;
            Status = status;
            OrderTime = DateTime.Now;
            TableNumber = tableNumber;
            OrderItems = orderItems;
        }*/
    }
}
