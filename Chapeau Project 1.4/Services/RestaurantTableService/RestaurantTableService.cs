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
                    var items = _orderRepo.GetOrderItemsByOrderNumber(order.OrderNumber);

                   // First, look for any ReadyToServe or BeingPrepared
                   bool foodHasReady = false, foodHasPreparing = false, foodHasServed = false;
                   bool drinkHasReady = false, drinkHasPreparing = false, drinkHasServed = false;

                   foreach (var item in items)
                   {
                      bool isDrink = 
                          item.MenuItem.Card != null 
                          && item.MenuItem.Card.Equals("Drinks", System.StringComparison.OrdinalIgnoreCase);

                      if (item.ItemStatus == EItemStatus.ReadyToServe)
                      {
                         if (isDrink)    drinkHasReady = true;
                          else            foodHasReady  = true;
                      }
                      else if (item.ItemStatus == EItemStatus.BeingPrepared)
                      {
                           if (isDrink)    drinkHasPreparing = true;
                           else            foodHasPreparing  = true;
                      }
                      else if (item.ItemStatus == EItemStatus.Served)
                      {
                          if (isDrink)    drinkHasServed = true;
                          else            foodHasServed  = true;
                      }
                   }

                   // Assign final statuses with priority: ReadyToServe > BeingPrepared > Served > None
                   if (foodHasReady)      vm.FoodOrderStatus = "ReadyToServe";
                   else if (foodHasPreparing) vm.FoodOrderStatus = "BeingPrepared";
                   else if (foodHasServed)    vm.FoodOrderStatus = "Served";

                   if (drinkHasReady)      vm.DrinkOrderStatus = "ReadyToServe";
                   else if (drinkHasPreparing) vm.DrinkOrderStatus = "BeingPrepared";
                   else if (drinkHasServed)    vm.DrinkOrderStatus = "Served";
                }

                result.Add(vm);
            }

            return result;
        }
    }
}
