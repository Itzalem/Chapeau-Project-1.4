using Chapeau_Project_1._4.Models;

namespace Chapeau_Project_1._4.Services
{
    public interface IPersonellService
    {
        Personell? GetByLoginCredentials(string userName, string password);
    }
}
