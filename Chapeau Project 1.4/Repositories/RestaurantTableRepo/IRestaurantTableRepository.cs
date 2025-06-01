using Chapeau_Project_1._4.Models;

namespace Chapeau_Project_1._4.Repositories.RestaurantTableRepo
{
    public interface IRestaurantTableRepository
    {
        List<RestaurantTable> GetAllTables();
        RestaurantTable? GetTableByNumber(int? table);
    }
}
