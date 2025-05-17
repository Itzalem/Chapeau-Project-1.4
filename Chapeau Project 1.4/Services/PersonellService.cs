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
        private readonly ILogger<PersonellService> _logger;

        public PersonellService(IPersonellRepository personellRepository, ILogger<PersonellService> logger)
        {
            _personellRepository = personellRepository;
            _logger = logger;
        }

        public Personell? GetByLoginCredentials(string userName, string password)
        {
            // Log the unhashed password for debugging (REMOVE IN PRODUCTION!)
            _logger.LogInformation($"Attempting login for username: {userName}");

            // Hash the password
            string hashedPassword = HashPassword(password);

            // Log the hashed password (REMOVE IN PRODUCTION!)
            _logger.LogInformation($"Hashed password: {hashedPassword}");

            // Get the user from the database
            Personell? personell = _personellRepository.GetByLoginCredentials(userName, hashedPassword);

            // Log whether the user was found
            _logger.LogInformation($"User found: {personell != null}");

            return personell;
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}