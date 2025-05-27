using Chapeau_Project_1._4.Models;
using Microsoft.Data.SqlClient;

namespace Chapeau_Project_1._4.Repositories.DrinkRepo
{
    public class DrinkRepository : IDrinkRepository
    {
        private readonly string? _connectionString;

        public DrinkRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("chapeaurestaurant");
        }

        public Drink GetDrinks(int? drinkId)
        {

            //the connection string was set in the constructor 
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT drink_id, drinkName, isAlcoholic, menuItem_id FROM DRINK WHERE @drinkId = drink_id";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@drinkId", drinkId);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                Drink drink = ReadDrink(reader);

                return drink;
            }
        }

        private Drink ReadDrink(SqlDataReader reader)
        {
            int DrinkId = (int)reader["drink_id"];
            string DrinkName = (string)reader["drinkName"];
            bool IsAlcoholic = (bool)reader["isAlcoholic"];
            int MenuItemId = (int)reader["menuItem_id"];

            return new Drink(DrinkId, DrinkName, IsAlcoholic, MenuItemId);
        }
    }
}
