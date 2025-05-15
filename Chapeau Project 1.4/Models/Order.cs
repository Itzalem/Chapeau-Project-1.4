using Microsoft.AspNetCore.Mvc;

namespace Chapeau_Project_1._4.Models
{

    public class Order
    {
        public int TableNumber { get; set; }
        public string MenuItemName { get; set; }
        public int Quantity { get; set; }
        public string Note { get; set; }
        public decimal Price { get; set; }
        public bool IsAlcoholic { get; set; }

        public Order()
        {
            TableNumber = 0;
            MenuItemName = "";
            Quantity = 0;
            Note = "";
            Price = 0;
        }

        public Order(int tableNumber, string menuItemName, int quantity, string note, decimal price, bool isAlcoholic)
        {
            TableNumber = tableNumber;
            MenuItemName = menuItemName;
            Quantity = quantity;
            Note = note;
            Price = price;
            IsAlcoholic = isAlcoholic;
        }

        public Order(int tableNumber, string menuItemName, int quantity, string note, decimal price)
        {
            TableNumber = tableNumber;
            MenuItemName = menuItemName;
            Quantity = quantity;
            Note = note;
            Price = price;
        }
    }
}
