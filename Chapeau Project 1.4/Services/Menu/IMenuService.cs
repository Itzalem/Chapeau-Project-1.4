using Chapeau_Project_1._4.Models;

namespace Chapeau_Project_1._4.Services.Menu
{
    public interface IMenuService
    {
        List<MenuItem> GetMenuItems(ECardOptions cardFilter, ECategoryOptions categoryFilter);
        Dictionary<ECardOptions, List<ECategoryOptions>> GetCardCategories();
        MenuItem GetMenuById(int? menuItemId);
    }
}
