using Chapeau_Project_1._4.Models;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace Chapeau_Project_1._4.Repositories
{
    public class PersonellRepository : IPersonellRepository
    {
        private readonly string _connectionString;

        public PersonellRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Personell? GetByLoginCredentials(string username, string password)
        {
            Personell? personell = null;

            // Hash the input password with SHA-256 before comparing it in the query
            string hashedPassword = HashPasswordSHA256(password);

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
        public string HashPasswordSHA256(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashBytes);
            }
        }

    }
}
