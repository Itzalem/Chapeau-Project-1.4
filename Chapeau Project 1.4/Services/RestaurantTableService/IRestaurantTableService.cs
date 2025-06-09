using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.Services.Order;
using Chapeau_Project_1._4.ViewModel;

namespace Chapeau_Project_1._4.Services.RestaurantTableService
{
    public interface IRestaurantTableService
    {
        List<RestaurantTable> GetAllTables();
        List<TableOrderViewModel> GetTablesWithOrderStatus();
        RestaurantTable? GetTableByNumber(int? table);
        void UpdateTableOccupancy(int tableNumber, bool isOccupied);

        void SetManualFreed(int tableNumber, bool wasFreed);

    }

}
