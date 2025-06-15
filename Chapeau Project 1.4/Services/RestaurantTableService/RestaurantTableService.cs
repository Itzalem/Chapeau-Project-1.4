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

                // Get active order
                OrderModel? order = _orderRepo.GetOrderByTable(table.TableNumber);
                if (order != null)
                {
                    var items = _orderRepo.GetOrderItemsByOrderNumber(order.OrderNumber);

                    bool hasFood = false, allFoodServed = true;
                    bool hasDrink = false, allDrinkServed = true;

                    foreach (var item in items)
                    {
                        bool isDrink = item.MenuItem.Card != null &&
                                       item.MenuItem.Card.Equals("Drinks", StringComparison.OrdinalIgnoreCase);

                        if (isDrink)
                        {
                            hasDrink = true;
                            if (item.ItemStatus == EItemStatus.ReadyToServe && vm.DrinkOrderStatus != "ReadyToServe")
                                vm.DrinkOrderStatus = "ReadyToServe";
                            else if (item.ItemStatus == EItemStatus.BeingPrepared && vm.DrinkOrderStatus == "None")
                                vm.DrinkOrderStatus = "Being-Prepared";
                            else if (item.ItemStatus == EItemStatus.pending && vm.DrinkOrderStatus == "None")
                                vm.DrinkOrderStatus = "Ordered";

                            if (item.ItemStatus != EItemStatus.Served)
                                allDrinkServed = false;
                        }
                        else
                        {
                            hasFood = true;
                            if (item.ItemStatus == EItemStatus.ReadyToServe && vm.FoodOrderStatus != "ReadyToServe")
                                vm.FoodOrderStatus = "ReadyToServe";
                            else if (item.ItemStatus == EItemStatus.BeingPrepared && vm.FoodOrderStatus == "None")
                                vm.FoodOrderStatus = "Being-Prepared";
                            else if (item.ItemStatus == EItemStatus.pending && vm.FoodOrderStatus == "None")
                                vm.FoodOrderStatus = "Ordered";

                            if (item.ItemStatus != EItemStatus.Served)
                                allFoodServed = false;
                        }
                    }

                    if (hasFood && allFoodServed)
                        vm.FoodOrderStatus = "Served";
                    if (hasDrink && allDrinkServed)
                        vm.DrinkOrderStatus = "Served";
                }
                else
                {
                    // No active order → reset status
                    vm.FoodOrderStatus = "None";
                    vm.DrinkOrderStatus = "None";
                }

                // Check if there is no order at all for this table (everything paid)
                bool hasActiveOrder = _orderRepo.GetOrderByTable(table.TableNumber) != null;
                if (!hasActiveOrder && table.IsOccupied && !table.WasManuallyFreed)
                {
                    _tableRepo.UpdateTableOccupancy(table.TableNumber, false); //mark table as free
                    _tableRepo.SetManualFreed(table.TableNumber, false); // reset manual flag
                    vm.IsOccupied = false;
                }


                result.Add(vm);
            }

            return result;
        }
        public void SetManualFreed(int tableNumber, bool wasFreed)
        {
            _tableRepo.SetManualFreed(tableNumber, wasFreed);
        }

        public bool CanBeFreed(RestaurantTable table, OrderModel? order)
        {
            if (!table.IsOccupied)
                return true;

            if (order == null)
                return true;

            if (order.Status == EOrderStatus.paid)
                return true;

            return false;
        }

        public bool TryToggleOccupancy(RestaurantTable table, OrderModel? order)
        {
            if (table.IsOccupied && order != null && order.Status != EOrderStatus.paid)
            {
                return false;
            }

            bool newStatus = !table.IsOccupied;

            SetManualFreed(table.TableNumber, !newStatus); // if we free it, set to true
            UpdateTableOccupancy(table.TableNumber, newStatus);

            return true;
        }


    }
}
