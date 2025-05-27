using Chapeau_Project_1._4.Models;

namespace Chapeau_Project_1._4.Repositories.MenuRepo
{
    public interface IMenuRepository
    {
        List<MenuItem> GetMenuItems(ECardOptions cardFilter, ECategoryOptions categoryFilter);

        MenuItem GetMenuItemById(int menuItemId);
    }
}
