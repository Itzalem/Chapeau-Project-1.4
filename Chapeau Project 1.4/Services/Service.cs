using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.Repositories;

namespace Chapeau_Project_1._4.Services
{
    public class Service : IMenuService
    {
        private IMenuRepository _menuRepository;

        public Service(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public List<MenuItem> DisplayMenu()
        {
            return _menuRepository.DisplayMenu();
        }
    }
}
