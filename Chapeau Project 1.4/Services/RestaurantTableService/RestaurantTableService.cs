using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.Repositories.OrderRepo;
using Chapeau_Project_1._4.Repositories.RestaurantTableRepo;
using Chapeau_Project_1._4.ViewModel;
using System.Collections.Generic;
using System.Diagnostics;

// Alias the model class so “OrderModel” is unambiguous
using OrderModel = Chapeau_Project_1._4.Models.Order;

namespace Chapeau_Project_1._4.Services.RestaurantTableService
{
    public class RestaurantTableService : IRestaurantTableService
    {
        private readonly IRestaurantTableRepository _tableRepo;
        private readonly IOrderRepository _orderRepo;

        // Constructor to inject table and order repositories
        public RestaurantTableService(IRestaurantTableRepository tableRepo, IOrderRepository orderRepo)
        {
            _tableRepo = tableRepo;
            _orderRepo = orderRepo;
        }

        // Returns a list of all restaurant tables from the database
        public List<RestaurantTable> GetAllTables()
        {
            try
            {
                return _tableRepo.GetAllTables();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[GetAllTables Error] {ex.Message}");
                return new List<RestaurantTable>();
            }
        }

        // Returns a specific table by its number (nullable for safety)
        public RestaurantTable? GetTableByNumber(int? table)
        {
            try
            {
                return _tableRepo.GetTableByNumber(table);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[GetTableByNumber Error] {ex.Message}");
                return null;
            }
        }

        // Updates whether a table is occupied or not
        public void UpdateTableOccupancy(int tableNumber, bool isOccupied)
        {
            try
            {
                _tableRepo.UpdateTableOccupancy(tableNumber, isOccupied);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[UpdateTableOccupancy Error] {ex.Message}");
            }
        }

        // Flags a table as manually freed (or not)
        public void SetManualFreed(int tableNumber, bool wasFreed)
        {
            try
            {
                _tableRepo.SetManualFreed(tableNumber, wasFreed);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[SetManualFreed Error] {ex.Message}");
            }
        }

        // Determines whether a table can be set to "free"
        // Rules:
        // - If not occupied, it's already free
        // - If there's no active order, it can be freed
        // - If the order exists and is paid, it can be freed
        public bool CanBeFreed(RestaurantTable table, OrderModel? order)
        {
            try
            {
                if (!table.IsOccupied)
                    return true;

                if (order == null)
                    return true;

                if (order.Status == EOrderStatus.paid)
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[CanBeFreed Error] {ex.Message}");
                return false;
            }
        }

        // Attempts to toggle table occupancy manually
        // If table has an unpaid order, it cannot be freed
        public bool TryToggleOccupancy(RestaurantTable table, OrderModel? order)
        {
            try
            {
                // Block toggling if there's an unpaid order
                if (table.IsOccupied && order != null && order.Status != EOrderStatus.paid)
                {
                    return false;
                }

                // Toggle status (true -> false or false -> true)
                bool newStatus = !table.IsOccupied;

                // If we're freeing it, mark that it was manually freed
                SetManualFreed(table.TableNumber, !newStatus);
                UpdateTableOccupancy(table.TableNumber, newStatus);

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[TryToggleOccupancy Error] {ex.Message}");
                return false;
            }
        }

        // Builds a list of table view models including order status (food/drink)
        public List<TableOrderViewModel> GetTablesWithOrderStatus()
        {
            try
            {
                var tables = _tableRepo.GetAllTables();
                var result = new List<TableOrderViewModel>();

                foreach (var table in tables)
                {
                    // Create a view model for each table using helper method
                    var vm = BuildViewModelForTable(table);
                    result.Add(vm);
                }

                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[GetTablesWithOrderStatus Error] {ex.Message}");
                return new List<TableOrderViewModel>();
            }
        }

        // -----------------------------
        // Private helper methods below
        // -----------------------------

        // Builds a view model for one table with food/drink status
        private TableOrderViewModel BuildViewModelForTable(RestaurantTable table)
        {
            try
            {
                var vm = new TableOrderViewModel
                {
                    TableNumber = table.TableNumber,
                    IsOccupied = table.IsOccupied,
                    FoodOrderStatus = "None",
                    DrinkOrderStatus = "None"
                };

                // Get the current order for the table, if any
                var order = _orderRepo.GetOrderByTable(table.TableNumber);

                // If order exists, set the food/drink statuses
                if (order != null)
                {
                    SetOrderStatuses(vm, order);
                }

                // Auto-reset table to free if needed
                ResetFreedTableIfNeeded(table, vm);

                return vm;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[BuildViewModelForTable Error] {ex.Message}");
                return new TableOrderViewModel
                {
                    TableNumber = table.TableNumber,
                    IsOccupied = false,
                    FoodOrderStatus = "None",
                    DrinkOrderStatus = "None"
                };
            }
        }

        // Sets the food and drink status values (e.g., "Served", "Being-Prepared", etc.)
        private void SetOrderStatuses(TableOrderViewModel vm, OrderModel order)
        {
            try
            {
                var items = _orderRepo.GetOrderItemsByOrderNumber(order.OrderNumber);

                bool hasFood = false, allFoodServed = true;
                bool hasDrink = false, allDrinkServed = true;

                foreach (var item in items)
                {
                    // Determine whether the item is a drink
                    bool isDrink = item.MenuItem.Card != null &&
                                   item.MenuItem.Card.Equals("Drinks", StringComparison.OrdinalIgnoreCase);

                    if (isDrink)
                    {
                        hasDrink = true;

                        // Update drink status depending on item progress
                        if (item.ItemStatus == EItemStatus.ReadyToServe && vm.DrinkOrderStatus != "ReadyToServe")
                            vm.DrinkOrderStatus = "ReadyToServe";
                        else if (item.ItemStatus == EItemStatus.BeingPrepared && vm.DrinkOrderStatus == "None")
                            vm.DrinkOrderStatus = "Being-Prepared";
                        else if (item.ItemStatus == EItemStatus.pending && vm.DrinkOrderStatus == "None")
                            vm.DrinkOrderStatus = "Ordered";

                        // If any drink isn't served yet, mark that
                        if (item.ItemStatus != EItemStatus.Served)
                            allDrinkServed = false;
                    }
                    else
                    {
                        hasFood = true;

                        // Update food status depending on item progress
                        if (item.ItemStatus == EItemStatus.ReadyToServe && vm.FoodOrderStatus != "ReadyToServe")
                            vm.FoodOrderStatus = "ReadyToServe";
                        else if (item.ItemStatus == EItemStatus.BeingPrepared && vm.FoodOrderStatus == "None")
                            vm.FoodOrderStatus = "Being-Prepared";
                        else if (item.ItemStatus == EItemStatus.pending && vm.FoodOrderStatus == "None")
                            vm.FoodOrderStatus = "Ordered";

                        // If any food isn't served yet, mark that
                        if (item.ItemStatus != EItemStatus.Served)
                            allFoodServed = false;
                    }
                }

                // If all food/drinks are served, mark them as "Served"
                if (hasFood && allFoodServed)
                    vm.FoodOrderStatus = "Served";
                if (hasDrink && allDrinkServed)
                    vm.DrinkOrderStatus = "Served";
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[SetOrderStatuses Error] {ex.Message}");
                vm.FoodOrderStatus = "Error";
                vm.DrinkOrderStatus = "Error";
            }
        }

        // Automatically frees a table if it's still marked as occupied but has no active orders
        private void ResetFreedTableIfNeeded(RestaurantTable table, TableOrderViewModel vm)
        {
            try
            {
                bool hasActiveOrder = _orderRepo.GetOrderByTable(table.TableNumber) != null;

                // If no active order and table is still marked as occupied (and not freed manually), update it
                if (!hasActiveOrder && table.IsOccupied && !table.WasManuallyFreed)
                {
                    _tableRepo.UpdateTableOccupancy(table.TableNumber, false); // set as free
                    _tableRepo.SetManualFreed(table.TableNumber, false);       // reset manual flag
                    vm.IsOccupied = false; // update the view model as well
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ResetFreedTableIfNeeded Error] {ex.Message}");
            }
        }
    }
}
