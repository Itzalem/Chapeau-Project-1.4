using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.Repositories;

namespace Chapeau_Project_1._4.Services
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

        public List<string> GetCategoriesByCard(string? cardFilter)
        {
            return _menuRepository.GetCategoriesByCard(cardFilter);
        }
    }
}
