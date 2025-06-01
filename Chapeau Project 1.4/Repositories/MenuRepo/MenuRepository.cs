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
           

            // Convertirt to string to dabatase can read them and convert again before filling the object
            string card = reader["menuCard"].ToString();
			string category = reader["category"].ToString();

            bool isAlcoholic = false;

            if (card == ECardOptions.Drinks.ToString())
                isAlcoholic = reader.GetBoolean(reader.GetOrdinal("isAlcoholic"));

            return new MenuItem(menuItem_id, menuItemName, price, stock, card, category, isAlcoholic);
        }

        public List<MenuItem> GetMenuItems(ECardOptions cardFilter, ECategoryOptions categoryFilter)
        {         
            List<MenuItem> menu = new List<MenuItem>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string selectFields = "MI.menuItem_id, MI.menuItemName, MI.price, MI.stock, MI.menuCard, MI.category";
                string fromClause = " FROM MENU_ITEMS AS MI";
                string whereClause = " WHERE MI.menuCard = @MenuCard";
                string orderClause = " ORDER BY MI.menuCard, MI.category";

                if (cardFilter == ECardOptions.Drinks)
                {
                    selectFields += ", D.isAlcoholic";
                    fromClause += " JOIN DRINK AS D ON MI.menuItem_id = D.menuItem_id";
                }

                if (categoryFilter != ECategoryOptions.All)
                {
                    whereClause += " AND MI.category = @Category";
                }

                string query = $"SELECT {selectFields}{fromClause}{whereClause}{orderClause};";

                SqlCommand command = new SqlCommand(query, connection);
                
                command.Parameters.AddWithValue("@MenuCard", cardFilter.ToString());                          

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


                if (reader.Read()) // Verifica que hay una fila antes de leer
                {
                    return ReadItem(reader); // Solo se llama si hay datos
                }

                // ❗ Si no hay datos, lanza excepción clara
                throw new Exception($"No MenuItem found with ID: {menuItemId}");
            }
        }

    }
}
