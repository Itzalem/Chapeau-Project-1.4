using Chapeau_Project_1._4.Models;

namespace Chapeau_Project_1._4.Repositories
{
    public interface IMenuRepository
    {
        List<MenuItem> DisplayMenu(string? cardFilter, string? categoryFilter);
        List<string> GetCategoriesByCard(string? cardFilter);
    }
}
