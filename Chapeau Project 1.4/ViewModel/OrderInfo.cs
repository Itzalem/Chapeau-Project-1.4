using Chapeau_Project_1._4.Models;

namespace Chapeau_Project_1._4.ViewModel
{
    public class OrderInfo
    {
        public Order order {  get; set; }
        public List<OrderItem> orderItem { get; set; }
        public List<MenuItem> menuItem { get; set; }
        public List<Drink> drink { get; set; }

        public OrderInfo(Order order, List<OrderItem> orderItem, List<MenuItem> menuItem, List<Drink> drink)
        {
            this.order = order;
            this.orderItem = orderItem;
            this.menuItem = menuItem;
            this.drink = drink;
        }
    }
}
