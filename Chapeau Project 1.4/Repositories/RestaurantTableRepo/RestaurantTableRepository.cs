﻿using Chapeau_Project_1._4.Models;
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
                string query = @"SELECT tableNumber, amountOfGuests, isOccupied FROM RESTAURANT_TABLE";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int TableNumber = reader.GetInt32(0);
                        int AmountOfGuests = reader.GetInt32(1);
                        bool IsOccupied = reader.GetBoolean(2);

						tables.Add(new RestaurantTable(TableNumber, AmountOfGuests, IsOccupied));
                    }
                }
            }

            return tables;
        }
    }

}
