using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.Repositories.PersonellRepo;
using System.Security.Cryptography;
using System.Text;

namespace Chapeau_Project_1._4.Services
{
    public class PersonellService : IPersonellService
    {
        private IPersonellRepository _personellRepository;

        public PersonellService(IPersonellRepository personellRepository)
        {
            _personellRepository = personellRepository;
        }

        public Personell? GetByLoginCredentials(string userName, string password)
        {
            return _personellRepository.GetByLoginCredentials(userName, HashPassword(password));
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
