namespace Chapeau_Project_1._4.Models
{
    public class MenuItem
    {
        public int MenuItemId { get; set; }
        public string MenuItemName { get; set; }
        public decimal Price { get; set; }
        public int Stock {  get; set; }

        //depending on the database changes i need to add "orderItem_id" or not

        public MenuItem()
        {
        }

        public MenuItem(int menuItemId, string menuItemName, decimal price, int stock)
        {
            MenuItemId = menuItemId;
            MenuItemName = menuItemName;
            Price = price;
            Stock = stock;
        }
        
    }
}
