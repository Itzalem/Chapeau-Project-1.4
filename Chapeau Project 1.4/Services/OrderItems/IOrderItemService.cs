﻿using Azure;
using Chapeau_Project_1._4.Models;
using Microsoft.AspNetCore.Mvc;

namespace Chapeau_Project_1._4.Services.OrderItems
{
    public interface IOrderItemService
    {
        List<OrderItem> DisplayItemsPerOrder(Chapeau_Project_1._4.Models.Order order);

        void AddOrderItem(OrderItem orderItem);
        void CheckDuplicateItems(Chapeau_Project_1._4.Models.Order order);

        public OrderItem GetOrderItemById(int orderItemId);     
               
        void UpdateHoldItemsStatus(Chapeau_Project_1._4.Models.Order order);

        void ReduceItemStock(Chapeau_Project_1._4.Models.Order order); 

        void EditItemQuantity(OrderItem orderItem, string operation);
        void DeleteSingleItem(OrderItem orderItem);
        void EditItemNote(OrderItem orderItem);


        /////
        List<OrderItem> GetByOrderNumber(int orderNumber);
        List<OrderItem> DisplayOrderItems();
        List<OrderItem> GetRunningItem();
        void UpdateItemStatus(int orderItemId, EItemStatus newStatus);
        void UpdateCourseStatus(int orderNumber, string category, ECategoryStatus categoryCourseStatus);

        //Lukas
        List<OrderItem> GetItemsForServing(int orderNumber);
        void ServeDrinkItems(int orderNumber);
        void ServeFoodItems(int orderNumber);
    }
}
