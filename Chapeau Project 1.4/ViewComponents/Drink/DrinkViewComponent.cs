﻿using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.Repositories.OrderItemRepo;
using Chapeau_Project_1._4.Repositories.OrderRepo;
using Chapeau_Project_1._4.Services.Order;
using Chapeau_Project_1._4.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Chapeau_Project_1._4.ViewComponents.Drink
{
    public class DrinkViewComponent: ViewComponent
    {
        private readonly IOrderService _ordersService;
        public DrinkViewComponent(
            IOrderService ordersService
            )
        {
            _ordersService = ordersService;
        }

        public IViewComponentResult Invoke(string filterValue)
        {
            var orders = _ordersService.GetOrdersWithItems(true , filterValue);
            if(string.IsNullOrEmpty(filterValue))
            {
                filterValue = "RunningOrders";
            }
            orders.ForEach(x => x.ShowCourse = filterValue != "RunningOrders");

            return View(orders);
        }
    }
}
