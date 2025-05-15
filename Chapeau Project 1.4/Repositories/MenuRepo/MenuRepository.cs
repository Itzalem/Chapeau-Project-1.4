using Chapeau_Project_1._4.Models;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace Chapeau_Project_1._4.Repositories.MenuRepo
{
    public class MenuRepository : IMenuRepository
    {
        private readonly string? _connectionString;

        public MenuRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("chapeaurestaurant");
        }

        private MenuItem ReadItem(SqlDataReader reader)
        {
            int menuItem_id = (int)reader["menuItem_id"];
            string menuItemName = (string)reader["menuItemName"];
            decimal price = (decimal)reader["price"];
            int stock = (int)reader["stock"];
            string card = (string)reader["menuCard"];
            string category = (string)reader["category"];
            string itemStatus = (string)reader["itemStatus"];

            return new MenuItem(menuItem_id, menuItemName, price, stock, card, category, itemStatus);
        }

        public List<MenuItem> DisplayMenu(ECardOptions cardFilter, ECategoryOptions categoryFilter)
        {
            List<MenuItem> menu = new List<MenuItem>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT menuItem_id, menuItemName, price, stock, menuCard, category, itemStatus " + //depending on changes add orderItem_id
                    " FROM MENU_ITEMS WHERE menuCard = @MenuCard";

                if (categoryFilter != ECategoryOptions.All) 
                {
                    query += " AND category = @Category";
                }

                query += "ORDER BY menuCard, category;";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@MenuCard", cardFilter);

                if (categoryFilter != ECategoryOptions.All)
                {
                    command.Parameters.AddWithValue("@Category", categoryFilter.ToString());
                }

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    MenuItem menuItem = ReadItem(reader);
                    menu.Add(menuItem);
                }
                reader.Close();

                return menu;
            }
        }
       
   
    }
}
