using Chapeau_Project_1._4.Models;

namespace Chapeau_Project_1._4.Services.RestaurantTableService
{
    public interface IRestaurantTableService
    {
        List<RestaurantTable> GetAllTables();
        RestaurantTable? GetTableByNumber(int? table);
    }

}
