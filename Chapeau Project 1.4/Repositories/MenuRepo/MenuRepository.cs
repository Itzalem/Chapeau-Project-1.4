using Chapeau_Project_1._4.Models;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Collections;

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

            // Convert to string to dabatase can read them and convert again before filling the object
            string card = reader["menuCard"].ToString();
			string category = reader["category"].ToString();

            bool isAlcoholic = false;

            if (card == ECardOptions.Drinks.ToString())
                isAlcoholic = reader.GetBoolean(reader.GetOrdinal("isAlcoholic"));

            return new MenuItem(menuItem_id, menuItemName, price, stock, card, category, isAlcoholic);
        }

        //gets all the menu items from the db and filters according to card and category 
        public List<MenuItem> GetMenuItems(ECardOptions cardFilter, ECategoryOptions categoryFilter)
        {
            List<MenuItem> menu = new List<MenuItem>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT menuItem_id, menuItemName, price, stock, menuCard, category, isAlcoholic " +
                               "FROM MENU_ITEMS " +
                               "WHERE menuCard = @MenuCard";

                if (categoryFilter != ECategoryOptions.All)
                {
                    query += " AND category = @Category";
                }

                query += " ORDER BY menuCard, category;";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@MenuCard", cardFilter.ToString());

                if (categoryFilter != ECategoryOptions.All)
                {
                    command.Parameters.AddWithValue("@Category", categoryFilter.ToString());
                }

                connection.Open();
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

        public MenuItem GetMenuItemById(int menuItemId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT menuItem_id, menuItemName, price, stock, menuCard, isAlcoholic, category " +
                    " FROM MENU_ITEMS " +
                    " WHERE menuItem_id = @menuItemId ";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@menuItemId", menuItemId);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();


                if (reader.Read()) 
                {
                    return ReadItem(reader); 
                }
                
                throw new Exception($"No MenuItem found with ID: {menuItemId}");
            }
        }

    }
}
