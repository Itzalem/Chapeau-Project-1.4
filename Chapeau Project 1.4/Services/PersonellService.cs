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

        public Personell? GetByLoginCredentials(string userName, string password)
        {

            string hashedPassword = HashPassword(password);

            // Get the user from the database
            Personell? personell = _personellRepository.GetByLoginCredentials(userName, hashedPassword);

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