namespace Chapeau_Project_1._4.Models
{
    public enum EItemStatus { pending , BeingPrepared, ReadyToServe }
    public class OrderItem
    {
        public int OrderItemId { get; set; }    
        public int Quantity { get; set; }
        public string Note { get; set; }    
        public EItemStatus ItemStatus { get; set; }  
        public int MenuItemId { get; set; } //pass the whole object 
        public string MenuItemName { get; set; } //pass the whole object 
        public int OrderNumber { get; set; }   
        

        public OrderItem()
        {

        }

        public OrderItem (int orderItemId, int quantity, string note, EItemStatus itemStatus, int menuItemId, int orderNumber , string menuItemName = "")
        {
            OrderItemId = orderItemId;
            Quantity = quantity;
            Note = note;
            ItemStatus = itemStatus;
            MenuItemName = menuItemName;
            MenuItemId = menuItemId;
            OrderNumber = orderNumber;
        }   
    }
}
