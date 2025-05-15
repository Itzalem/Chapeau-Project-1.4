using Microsoft.Data.SqlClient;
using Chapeau_Project_1._4.Models;

namespace Chapeau_Project_1._4.Repositories.OrderOverviewRepo
{
    public class OrderOverviewRepository : IOrderOverviewRepository
    {
        private readonly string? _connectionString;

        public OrderOverviewRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("chapeaurestaurant");
        }

        public List<OverviewItem> DisplayOrderDrinks()
        {
            List<OverviewItem> order = new List<OverviewItem>();

            //the connection string was set in the constructor 
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query =  "SELECT DISTINCT tableNumber, menuItemName, isAlcoholic, quantity, note, price" +
                                "FROM dbo.ORDER_ITEM AS OI " +
                                    "JOIN dbo.MENU_ITEMS AS MI ON OI.menuItem_id = MI.menuItem_id" +
                                    "JOIN dbo.ORDERS AS O ON O.orderNumber = OI.orderNumber " +
                                    "JOIN dbo.DRINK AS D ON  D.menuItem_id = OI.menuItem_id " +
                                "WHERE tableNumber = 1 AND[status] NOT LIKE 'payed' AND category LIKE 'drink'";
                SqlCommand command = new SqlCommand(query, connection);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    OverviewItem orderItem = ReadDrinkItem(reader);
                    order.Add(orderItem);
                }
                reader.Close();

            }
            return order;
        }

        private OverviewItem ReadDrinkItem(SqlDataReader reader)
        {
            int TableNumber = (int)reader["tableNumber"];
            string MenuItemName = (string)reader["menuItemName"];
            int Quantity = (int)reader["quantity"];
            string Note = (string)reader["note"];
            decimal Price = (decimal)reader["price"];
            bool IsAlcoholic = (bool)reader["isAlcoholic"];

            return new OverviewItem(TableNumber, MenuItemName, Quantity, Note, Price, IsAlcoholic);
        }

        public List<OverviewItem> DisplayOrderDishes()
        {
            List<OverviewItem> order = new List<OverviewItem>();

            //the connection string was set in the constructor 
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT DISTINCT tableNumber, menuItemName, quantity, note, price " +
                                "FROM dbo.ORDER_ITEM AS OI " +
                                    "JOIN dbo.MENU_ITEMS AS MI ON OI.menuItem_id = MI.menuItem_id " +
                                    "JOIN dbo.ORDERS AS O ON O.orderNumber = OI.orderNumber " +
                                    "JOIN dbo.DISH AS D ON  D.menuItem_id = OI.menuItem_id" +
                                "WHERE tableNumber = 1 AND[status] NOT LIKE 'payed' AND category LIKE 'dish';";
                SqlCommand command = new SqlCommand(query, connection);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    OverviewItem orderItem = ReadDishItem(reader);
                    order.Add(orderItem);
                }
                reader.Close();
            }
            return order;
        }

        private OverviewItem ReadDishItem(SqlDataReader reader)
        {
            int TableNumber = (int)reader["tableNumber"];
            string MenuItemName = (string)reader["menuItemName"];
            int Quantity = (int)reader["quantity"];
            string Note = (string)reader["note"];
            decimal Price = (decimal)reader["price"];

            return new OverviewItem(TableNumber, MenuItemName, Quantity, Note, Price);
        }
    }
}
