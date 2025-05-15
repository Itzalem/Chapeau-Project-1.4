using Chapeau_Project_1._4.Models;

namespace Chapeau_Project_1._4.Services.Menu
{
    public interface IMenuService
    {
        List<MenuItem> DisplayMenu(ECardOptions cardFilter, ECategoryOptions categoryFilter);



        
    }
}
