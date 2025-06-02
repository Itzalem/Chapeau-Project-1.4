namespace Chapeau_Project_1._4.Models
{
    public enum ECategoryStatus { pending, InProcess, Completed } 
    public class MenuItem
    {
        public int MenuItemId { get; set; }
        public string MenuItemName { get; set; }
        public decimal Price { get; set; }
        public int Stock {  get; set; }
        public string Card {  get; set; }
        public string Category { get; set; }
        public bool IsAlcoholic { get; set; }
        public ECategoryStatus CategoryStatus { get; set; } 
    

        public MenuItem()
        {
        }

        public MenuItem(int menuItemId, string menuItemName, decimal price, int stock, 
                        string card, string category, bool isAlcoholic, ECategoryStatus categoryStatus = ECategoryStatus.pending)
        {
            MenuItemId = menuItemId;
            MenuItemName = menuItemName;
            Price = price;
            Stock = stock;
            Card = card;
            Category = category;
            IsAlcoholic = isAlcoholic;
            CategoryStatus = categoryStatus;
        }
        public MenuItem(int menuItemId, string menuItemName,string category, ECategoryStatus categoryStatus = ECategoryStatus.pending)
        {
            MenuItemId = menuItemId;
            MenuItemName = menuItemName;
            Category = category;
            CategoryStatus = categoryStatus;
        }
    }
}
