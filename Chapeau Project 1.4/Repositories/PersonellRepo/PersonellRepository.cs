using Chapeau_Project_1._4.Models;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace Chapeau_Project_1._4.Repositories.PersonellRepo
{
    public class PersonellRepository : IPersonellRepository
    {
        private readonly string? _connectionString;

        public PersonellRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("chapeaurestaurant");
        }

        public Personell? GetByLoginCredentials(string username, string hashedPassword)
        {
            Personell? personell = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
                            SELECT staff_id, username, password, role
                            FROM PERSONELL
                            WHERE username = @Username AND password = @Password";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", hashedPassword);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        personell = new Personell
                        {
                            Staff_id = reader.GetInt32(reader.GetOrdinal("staff_id")),
                            Username = reader.GetString(reader.GetOrdinal("username")),
                            Password = reader.GetString(reader.GetOrdinal("password")),
                            Role = reader.GetString(reader.GetOrdinal("role"))
                        };
                    }
                }
            }

            return personell;
        }

    }
}
