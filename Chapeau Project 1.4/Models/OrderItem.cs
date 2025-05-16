namespace Chapeau_Project_1._4.Models
{
    public enum ItemStatus { Ordered, BeingPrepared, ReadyToServe }
    public class OrderItem
    {
        public int OrderItemId { get; set; }    
        public int Quantity { get; set; }
        public string Note { get; set; }    
        public ItemStatus ItemStatus { get; set; }  
        public int MenuItemId { get; set; } 
        public int OrderNumber { get; set; }  
        

        public OrderItem()
        {

        }

        public OrderItem (int orderItemId, int quantity, string note, ItemStatus itemStatus, int menuItemId, int orderNumber)
        {
            OrderItemId = orderItemId;
            Quantity = quantity;
            Note = note;
            ItemStatus = itemStatus;
            MenuItemId = menuItemId;
            OrderNumber = orderNumber;
        }   
    }
}
