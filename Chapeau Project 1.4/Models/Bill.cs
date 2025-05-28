namespace Chapeau_Project_1._4.Models
{
    public class Bill
    {
        public int BillId { get; set; }
        public decimal Tip { get; set; }
        public Order Order { get; set; }
        public RestaurantTable Table { get; set; }

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
    }
}
