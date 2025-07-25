﻿namespace Chapeau_Project_1._4.Models
{
    public enum EItemStatus {onHold, pending , BeingPrepared, ReadyToServe, Served }
    public class OrderItem
    {
        public int OrderItemId { get; set; }    
        public int Quantity { get; set; }
        public string Note { get; set; }    
        public EItemStatus ItemStatus { get; set; }
        
        public MenuItem MenuItem { get; set; } //pass the whole object 
        public int OrderNumber { get; set; }   

        //public int MenuItemId { get; set; }
        

        public OrderItem()
        {

        }

        public OrderItem (int orderItemId, int quantity, string note, EItemStatus itemStatus, MenuItem menuItem, int orderNumber)
        {
            OrderItemId = orderItemId;
            Quantity = quantity;
            Note = note;
            ItemStatus = itemStatus;    
            MenuItem = menuItem;
            OrderNumber = orderNumber;
        }

       
    }
}
