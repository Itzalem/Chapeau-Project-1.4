using Chapeau_Project_1._4.Models;

namespace Chapeau_Project_1._4.Services
{
    public interface IMenuService
    {
        List<MenuItem> DisplayMenu(string? cardFilter, string? categoryFilter);
        List<string> GetCategoriesByCard(string? cardFilter);
    }
}
