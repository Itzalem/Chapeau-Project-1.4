using Chapeau_Project_1._4.Models;
using Microsoft.Data.SqlClient;

namespace Chapeau_Project_1._4.Repositories.RestaurantTableRepo
{
    public class RestaurantTableRepository : IRestaurantTableRepository
    {
        private readonly string? _connectionString;

        public RestaurantTableRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("chapeaurestaurant");
        }

        public List<RestaurantTable> GetAllTables()
        {
            List<RestaurantTable> tables = new();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"SELECT tableNumber, isOccupied, wasManuallyFreed FROM RESTAURANT_TABLE";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int tableNumber = reader.GetInt32(0);
                        bool isOccupied = reader.GetBoolean(1);
                        bool wasManuallyFreed = reader.GetBoolean(2);

                        tables.Add(new RestaurantTable
                        {
                            TableNumber = tableNumber,
                            IsOccupied = isOccupied,
                            WasManuallyFreed = wasManuallyFreed
                        });
                    }
                }
            }

            return tables;
        }

        public RestaurantTable? GetTableByNumber(int? table)
        {
            RestaurantTable? restaurantTable = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"SELECT tableNumber, isOccupied, wasManuallyFreed FROM RESTAURANT_TABLE WHERE tableNumber = @table";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@table", table);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        restaurantTable = new RestaurantTable
                        {
                            TableNumber = reader.GetInt32(0),
                            IsOccupied = reader.GetBoolean(1),
                            WasManuallyFreed = reader.GetBoolean(2)
                        };
                    }
                }
            }

            return restaurantTable;
        }

        public void UpdateTableOccupancy(int tableNumber, bool isOccupied)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "UPDATE RESTAURANT_TABLE SET isOccupied = @isOccupied WHERE tableNumber = @tableNumber";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@isOccupied", isOccupied);
                cmd.Parameters.AddWithValue("@tableNumber", tableNumber);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            // Track that the user toggled manually
            SetManualFreed(tableNumber, true);
        }

        public void SetManualFreed(int tableNumber, bool wasFreed)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE RESTAURANT_TABLE
                                 SET wasManuallyFreed = @wasFreed
                                 WHERE tableNumber = @tableNumber";

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@wasFreed", wasFreed);
                cmd.Parameters.AddWithValue("@tableNumber", tableNumber);
                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
