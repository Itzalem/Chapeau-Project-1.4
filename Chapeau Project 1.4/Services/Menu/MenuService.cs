using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.Repositories.MenuRepo;
using Microsoft.AspNetCore.Cors.Infrastructure;
using System.Collections.Generic;

namespace Chapeau_Project_1._4.Services.Menu
{
    public class MenuService : IMenuService
    {
        private IMenuRepository _menuRepository;

        public MenuService(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public List<MenuItem> GetMenuItems(ECardOptions cardFilter, ECategoryOptions categoryFilter)
        {
            return _menuRepository.GetMenuItems(cardFilter, categoryFilter);
        }

        //si tengo tiempo automatizo el llenado de la lista
        private readonly Dictionary<ECardOptions, List<ECategoryOptions>> categoriesByCard = 
            new Dictionary<ECardOptions, List<ECategoryOptions>>        
            {
                {ECardOptions.Lunch, new  List<ECategoryOptions>
                    {
                       ECategoryOptions.All, ECategoryOptions.Starters, ECategoryOptions.Mains, ECategoryOptions.Desserts
                    }
                },
                {ECardOptions.Dinner, new  List<ECategoryOptions>
                    {
                       ECategoryOptions.All, ECategoryOptions.Starters, ECategoryOptions.Entremets, ECategoryOptions.Mains, ECategoryOptions.Desserts
                    }
                },
                {ECardOptions.Drinks, new  List<ECategoryOptions>
                    {
                        ECategoryOptions.All, ECategoryOptions.SoftDrinks, ECategoryOptions.Beers, ECategoryOptions.Wines, 
                        ECategoryOptions.Spirits, ECategoryOptions.Coffees, ECategoryOptions.Teas
                    }
                }

            };

        public Dictionary<ECardOptions, List<ECategoryOptions>> GetCardCategories()
        {
            return categoriesByCard;
        }

        public MenuItem GetMenuById(int? menuItemId)
        {
            return _menuRepository.GetMenuById(menuItemId);
        }
    }
}
