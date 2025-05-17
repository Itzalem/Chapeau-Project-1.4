using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.Repositories.RestaurantTableRepo;

namespace Chapeau_Project_1._4.Services.RestaurantTableService
{
    public class RestaurantTableService : IRestaurantTableService
    {
        private readonly IRestaurantTableRepository _repo;

        public RestaurantTableService(IRestaurantTableRepository repo)
        {
            _repo = repo;
        }

        public List<RestaurantTable> GetAllTables()
        {
            return _repo.GetAllTables();
        }
    }

}
