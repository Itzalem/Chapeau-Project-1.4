using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.Services.Order;
using Chapeau_Project_1._4.ViewModel;

// Alias the model class so “OrderModel” is unambiguous
using OrderModel = Chapeau_Project_1._4.Models.Order;

namespace Chapeau_Project_1._4.Services.RestaurantTableService
{
    public interface IRestaurantTableService
    {
        List<RestaurantTable> GetAllTables();
        List<TableOrderViewModel> GetTablesWithOrderStatus();
        RestaurantTable? GetTableByNumber(int? table);
        void UpdateTableOccupancy(int tableNumber, bool isOccupied);

        void SetManualFreed(int tableNumber, bool wasFreed);

        bool TryToggleOccupancy(RestaurantTable table, OrderModel? order);

        bool CanBeFreed(RestaurantTable table, OrderModel? order);

    }

}
