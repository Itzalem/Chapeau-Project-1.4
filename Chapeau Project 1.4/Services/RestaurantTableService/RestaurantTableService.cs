using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.Repositories.RestaurantTableRepo;
using Chapeau_Project_1._4.Services.Order;
using Chapeau_Project_1._4.ViewModel;

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

        public List<TableOrderViewModel> GetTablesWithOrderStatus(IOrderService orderService)
        {
            List<RestaurantTable> tables = _repo.GetAllTables();
            List<TableOrderViewModel> result = new();

            foreach (var table in tables)
            {
                var viewModel = new TableOrderViewModel
                {
                    TableNumber = table.TableNumber,
                    AmountOfGuests = table.AmountOfGuests,
                    IsOccupied = table.IsOccupied,
                    FoodOrderStatus = "None",
                    DrinkOrderStatus = "None"
                };

                var order = orderService.GetOrderByTable(table.TableNumber);
                if (order != null)
                {
                    var items = orderService.GetOrderItems(order.OrderNumber);

                    foreach (var item in items)
                    {
                        // Parse string category to enum
                        bool parsed = Enum.TryParse<ECategoryOptions>(item.MenuItem.Category, true, out var cat);

                        if (parsed)
                        {
                            // Check categories that are drinks
                            if (cat == ECategoryOptions.Coffees || cat == ECategoryOptions.SoftDrinks || cat == ECategoryOptions.Spirits ||
                                cat == ECategoryOptions.Beers || cat == ECategoryOptions.Wines || cat == ECategoryOptions.Teas)
                            {
                                if (item.ItemStatus == EItemStatus.ReadyToServe)
                                    viewModel.DrinkOrderStatus = "ReadyToServe";
                                else if (item.ItemStatus == EItemStatus.BeingPrepared && viewModel.DrinkOrderStatus != "ReadyToServe")
                                    viewModel.DrinkOrderStatus = "BeingPrepared";
                            }
                            else
                            {
                                // Everything else is food
                                if (item.ItemStatus == EItemStatus.ReadyToServe)
                                    viewModel.FoodOrderStatus = "ReadyToServe";
                                else if (item.ItemStatus == EItemStatus.BeingPrepared && viewModel.FoodOrderStatus != "ReadyToServe")
                                    viewModel.FoodOrderStatus = "BeingPrepared";
                            }
                        }
                        else
                        {
                            // Fallback: treat unknown categories as food
                            if (item.ItemStatus == EItemStatus.ReadyToServe)
                                viewModel.FoodOrderStatus = "ReadyToServe";
                            else if (item.ItemStatus == EItemStatus.BeingPrepared && viewModel.FoodOrderStatus != "ReadyToServe")
                                viewModel.FoodOrderStatus = "BeingPrepared";
                        }
                    }
                }

                result.Add(viewModel);
            }

            return result;
        }

        public RestaurantTable? GetTableByNumber(int? table)
        {
            return _repo.GetTableByNumber(table);
        }

        public void UpdateTableOccupancy(int tableNumber, bool isOccupied)
        {
            _repo.UpdateTableOccupancy(tableNumber, isOccupied);
        }


    }

}
