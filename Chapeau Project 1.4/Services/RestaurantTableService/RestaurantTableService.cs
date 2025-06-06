using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.Repositories.OrderRepo;
using Chapeau_Project_1._4.Repositories.RestaurantTableRepo;
using Chapeau_Project_1._4.ViewModel;
using System.Collections.Generic;

// Alias the model class so “OrderModel” is unambiguous
using OrderModel = Chapeau_Project_1._4.Models.Order;

namespace Chapeau_Project_1._4.Services.RestaurantTableService
{
    public class RestaurantTableService : IRestaurantTableService
    {
        private readonly IRestaurantTableRepository _tableRepo;
        private readonly IOrderRepository _orderRepo;

        public RestaurantTableService(
            IRestaurantTableRepository tableRepo,
            IOrderRepository orderRepo)
        {
            _tableRepo = tableRepo;
            _orderRepo = orderRepo;
        }

        public List<RestaurantTable> GetAllTables()
        {
            return _tableRepo.GetAllTables();
        }

        public RestaurantTable? GetTableByNumber(int? table)
        {
            return _tableRepo.GetTableByNumber(table);
        }

        public void UpdateTableOccupancy(int tableNumber, bool isOccupied)
        {
            _tableRepo.UpdateTableOccupancy(tableNumber, isOccupied);
        }

        public List<TableOrderViewModel> GetTablesWithOrderStatus()
        {
            var tables = _tableRepo.GetAllTables();
            var result = new List<TableOrderViewModel>();

            foreach (var table in tables)
            {
                var vm = new TableOrderViewModel
                {
                    TableNumber = table.TableNumber,
                    IsOccupied = table.IsOccupied,
                    FoodOrderStatus = "None",
                    DrinkOrderStatus = "None"
                };

                // Fetch the active Order (status pending/inProgress/prepared) for this table
                OrderModel? order = _orderRepo.GetOrderByTable(table.TableNumber);
                if (order != null)
                {
                    // Fetch that order’s items
                    List<OrderItem> items = _orderRepo.GetOrderItemsByOrderNumber(order.OrderNumber);

                    foreach (var item in items)
                    {
                        bool isDrink =
                            item.MenuItem.Card != null
                            && item.MenuItem.Card.Equals("Drinks", StringComparison.OrdinalIgnoreCase);

                        // 1) If any single item is ReadyToServe → that category = "ReadyToServe"
                        if (item.ItemStatus == EItemStatus.ReadyToServe)
                        {
                            if (isDrink)
                                vm.DrinkOrderStatus = "ReadyToServe";
                            else
                                vm.FoodOrderStatus = "ReadyToServe";
                        }
                        // 2) Else if any single item is BeingPrepared (and we haven't already set ReadyToServe)
                        else if (item.ItemStatus == EItemStatus.BeingPrepared)
                        {
                            if (isDrink && vm.DrinkOrderStatus != "ReadyToServe")
                                vm.DrinkOrderStatus = "Being-Prepared";
                            if (!isDrink && vm.FoodOrderStatus != "ReadyToServe")
                                vm.FoodOrderStatus = "Being-Prepared";
                        }
                        // 3) Else if any single item is pending (and we haven't set a higher state already)
                        else if (item.ItemStatus == EItemStatus.pending)
                        {
                            if (isDrink && vm.DrinkOrderStatus == "None")
                                vm.DrinkOrderStatus = "Ordered";
                            if (!isDrink && vm.FoodOrderStatus == "None")
                                vm.FoodOrderStatus = "Ordered";
                        }
                    }
                }

                result.Add(vm);
            }

            return result;
        }
    }
}
