using Chapeau_Project_1._4.Models;

namespace Chapeau_Project_1._4.Repositories
{
    public interface IPersonellRepository
    {
        Personell? GetByLoginCredentials(string username, string password);
    }
}
