using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.Repositories.MenuRepo;

namespace Chapeau_Project_1._4.Services.Menu
{
    public class MenuService : IMenuService
    {
        private IMenuRepository _menuRepository;

        public MenuService(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public List<MenuItem> DisplayMenu(string? cardFilter, string? categoryFilter)
        {
            return _menuRepository.DisplayMenu(cardFilter, categoryFilter);
        }

    }
}
