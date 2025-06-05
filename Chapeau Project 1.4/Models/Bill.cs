namespace Chapeau_Project_1._4.Models
{
    public class Bill
    {
        public int BillId { get; set; }
        public decimal Tip { get; set; }
        public Order Order { get; set; } = new Order();
        public RestaurantTable Table { get; set; } = new RestaurantTable();

        public Bill()
        {
            
        }

        public Bill(int billId, decimal tip, Order order, RestaurantTable table)
        {
            BillId = billId;
            Tip = tip;
            Order = order;
            Table = table;
        }

        public Bill(Order order, RestaurantTable table)
        {
            BillId = 0;
            Tip = 0;
            Order = order;
            Table = table;
        }
    }
}
