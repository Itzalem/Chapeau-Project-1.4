using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.Repositories.PersonellRepo;
using System.Security.Cryptography;
using System.Text;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Chapeau_Project_1._4.Services
{
    public class PersonellService : IPersonellService
    {
        private readonly IPersonellRepository _personellRepository;

        public PersonellService(IPersonellRepository personellRepository)
        {
            _personellRepository = personellRepository;
        }

        // Try to log in with given username and password.
        // If successful, return the Personell object and set controller/action using out parameters.
        public Personell TryLogin(string username, string password, out string redirectController, out string redirectAction)
        {
            // Default to login page (in case login fails)
            redirectController = "Personell";
            redirectAction = "Login";

            try
            {
                // Hash the entered password to match what's stored in the database
                string hashedPassword = HashPassword(password);

                // Check the credentials against the database
                Personell personell = _personellRepository.GetByLoginCredentials(username, hashedPassword);

                // If not found, return null and keep default redirect
                if (personell == null)
                {
                    return null;
                }

                // Based on the user's role, decide where to redirect
                if (personell.Role == "waiter")
                {
                    redirectController = "RestaurantTable";
                    redirectAction = "Overview";
                }
                else if (personell.Role == "kitchen")
                {
                    redirectController = "Kitchen";
                    redirectAction = "Index";
                }
                else if (personell.Role == "bar")
                {
                    redirectController = "Bar";
                    redirectAction = "Index";
                }

                // Return the logged-in personell
                return personell;
            }
            catch (Exception ex)
            {
                // Log or handle the error if needed (can be extended later)
                Debug.WriteLine($"[Login Error] {ex.Message}");
                return null;
            }
        }

        private string HashPassword(string password)
        {
            try
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                    return Convert.ToBase64String(hashBytes);
                }
            }
            catch (Exception ex)
            {
                // Log or handle the error if hashing fails
                Debug.WriteLine($"[Hashing Error] {ex.Message}");
                return string.Empty;
            }
        }
    }
}
