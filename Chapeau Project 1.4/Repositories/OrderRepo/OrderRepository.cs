﻿using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.Services.OrderItems;
using Chapeau_Project_1._4.ViewModel;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace Chapeau_Project_1._4.Repositories.OrderRepo
{
    public class OrderRepository : IOrderRepository
    {

        private readonly string? _connectionString;


        public OrderRepository(IConfiguration configuration, IOrderItemService orderItemService)
        {
            // get (database connectionstring from appsetings 
            _connectionString = configuration.GetConnectionString("ChapeauRestaurant");
        }
        private Order ReadOrder(SqlDataReader reader)
        {
            int OrderNumber = (int)reader["orderNumber"];
            EOrderStatus Status = (EOrderStatus)Enum.Parse(typeof(EOrderStatus), reader["status"].ToString()!, true);
            DateTime OrderTime = (DateTime)reader["orderTime"];
            int TableNumber = (int)reader["tableNumber"];

            return new Order(OrderNumber, Status, OrderTime, TableNumber);
        }

        public List<Order> DisplayOrder()
        {
            List<Order> orders = new List<Order>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT orderNumber, status, orderTime ,tableNumber 
                                 FROM ORDERS 
                                 WHERE status <> 'onHold' ORDER BY orderTime ASC";
                SqlCommand command = new SqlCommand(query, connection);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                // for each row in the orders table, it converts it into an object  
                while (reader.Read())
                {                    
                    var order = ReadOrder(reader);
                    orders.Add(order);
                }
                reader.Close();

            }
            return orders;
        }

        public int AddNewOrder(int tableNumber) //M
            //returns the new added order Id
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = $"INSERT INTO ORDERS (status, tableNumber, orderTime) " +
                                $" VALUES (@Status, @TableNumber, @OrderTime); " +
                                $"SELECT SCOPE_IDENTITY(); ";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Status", EOrderStatus.onHold.ToString());
                command.Parameters.AddWithValue("@TableNumber", tableNumber);
                command.Parameters.AddWithValue("@OrderTime", DateTime.Now);

                command.Connection.Open();

                object orderId = command.ExecuteScalar();
                if (orderId == null || orderId == DBNull.Value)
                {
                    throw new Exception("Failed to get order ID");
                }

                return Convert.ToInt32(orderId);

            }

        }

        public Order? GetOrderByNumber(int orderNumber)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT orderNumber, status, tableNumber, orderTime FROM Orders WHERE orderNumber = @orderNumber ";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@orderNumber", orderNumber);

                command.Connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return ReadOrder(reader);
                }
            }
            return null;
        }


        public Order? GetOrderByTable(int? table) 
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT orderNumber, status, tableNumber, orderTime 
                                 FROM ORDERS 
                                 WHERE tableNumber = @table
                                 AND status IN ('onHold', 'pending', 'inProgress', 'prepared')";
                // lukas: i added the onHold

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@table", table);

                command.Connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return ReadOrder(reader);
                }
            }
            return null;
        }

        public void UpdateOrderStatus(EOrderStatus status, int orderNumber)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE Orders SET status = @status , finishOrderTime = @finishTime WHERE orderNumber = @orderNumber";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@status", status.ToString());
                command.Parameters.AddWithValue("@finishTime", DateTime.Now);
                command.Parameters.AddWithValue("@orderNumber", orderNumber);

                command.Connection.Open();
                int rowChanged = command.ExecuteNonQuery();
                if (rowChanged == 0)
                {
                    throw new Exception("Failed to update the order");
                }
            }
        }

        public void CancelUnsentOrder(Order order) //M
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"DELETE FROM ORDER_ITEM 
                                 WHERE itemStatus = @Status and orderNumber = @OrderNumber ";


                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Status", EItemStatus.onHold.ToString());
                command.Parameters.AddWithValue("@OrderNumber", order.OrderNumber);

                command.Connection.Open();
                int rowsAffected = command.ExecuteNonQuery();

                int itemsInList = CountItemsInOrder(order);

                if (itemsInList == 0) //if the order item list is empty, the order will be deleted
                {
                    DeleteEmptyOrder(order);
                }
            }
        }

        public void DeleteEmptyOrder(Order order) //M
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {

                    string query = "DELETE FROM ORDERS WHERE orderNumber = @OrderNumber";

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@OrderNumber", order.OrderNumber);

                    command.Connection.Open();

                    command.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting empty order", ex);
            }
        }

        public int CountItemsInOrder(Order order)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT COUNT(*) FROM ORDER_ITEM WHERE orderNumber = @OrderNumber; ";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@OrderNumber", order.OrderNumber);

                command.Connection.Open();

                return (int)command.ExecuteScalar();
            }
        }

        public void Update(Order order)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {

                string query = @"UPDATE Orders SET status = @status, tableNumber = @tableNumber, orderTime = @orderTime WHERE orderNumber = @orderNumber";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@status", order.Status.ToString());
                command.Parameters.AddWithValue("@tableNumber", order.TableNumber);
                command.Parameters.AddWithValue("@orderTime", order.OrderTime);
                command.Parameters.AddWithValue("@orderNumber", order.OrderNumber);

                command.Connection.Open();
                command.ExecuteNonQuery();
            }

        }
        
        // To get the name of each orderItem from the MenuItems 
        public object GetOrderMunuItemName(int OrderNumber)
        {
            List<object> orders = new List<object>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                #region Query TXT
                string query = @"SELECT 
                                mi.menuItem_id,
                                mi.menuItemName,
                                mi.category,
	                            mi.categoryStatus
                            FROM 
                                Orders o
                            INNER JOIN 
                              ORDER_ITEM oi ON o.orderNumber = oi.orderNumber
                            INNER JOIN 
                                MENU_ITEMS mi ON oi.menuItem_id = mi.menuItem_id
                            WHERE 
                                o.orderNumber = @OrderNumber
                            ORDER BY o.orderTime;";

                #endregion
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@OrderNumber", OrderNumber);
                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    object order = ReadOrderGraph(reader);
                    return order;
                }
                reader.Close();

            }
            return orders;

        }

        //To read MenuItemName and the category - Mania 
        private object ReadOrderGraph(SqlDataReader reader)
        {
            int orderItemId = (int)reader["orderItem_id"];
            int menuItemId = (int)reader["menuItem_id"];
            string menuItemName = (string)reader["menuItemName"];
            string category = (string)reader["category"];
            ECategoryStatus categoryStatus = (ECategoryStatus)Enum.Parse(typeof(ECategoryStatus), reader["categoryStatus"].ToString()!);

            return new { OrderItemId = orderItemId, MenuItemId = menuItemId, Category = category, CategoryStatus = categoryStatus };
        }


        //for the overview - Lukas
        public List<OrderItem> GetOrderItemsByOrderNumber(int orderNumber)
        {
            // Create a list to hold all order items found for this order
            List<OrderItem> items = new List<OrderItem>();

            // Open a connection to the database using the configured connection string
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                // SQL query to select order item details and related menu item info
                string query = @"
                  SELECT 
                    oi.orderItem_id,
                    oi.itemStatus,
                    oi.quantity,
                    ISNULL(oi.note, '') AS note,             -- Replace NULL notes with empty string
                    mi.menuItem_id,
                    mi.menuItemName,
                    mi.price,
                    mi.stock,
                    mi.menuCard,
                    mi.category,
                    mi.isAlcoholic
                  FROM ORDER_ITEM oi
                  INNER JOIN MENU_ITEMS mi 
                    ON oi.menuItem_id = mi.menuItem_id       -- Join to get menu item details
                  WHERE oi.orderNumber = @orderNumber        -- Filter by the given order number
                ";

                // Prepare the SQL command with the query and add the parameter
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@orderNumber", orderNumber);

                // Open the database connection
                conn.Open();

                // Execute the query and read the result rows
                SqlDataReader reader = cmd.ExecuteReader();

                // Loop through all rows in the result set
                while (reader.Read())
                {
                    // Create a MenuItem object and fill it with data from the reader
                    MenuItem menuItem = new MenuItem
                    {
                        MenuItemId = (int)reader["menuItem_id"],
                        MenuItemName = reader["menuItemName"].ToString()!,
                        Price = (decimal)reader["price"],
                        Stock = (int)reader["stock"],
                        Card = reader["menuCard"].ToString()!,
                        Category = reader["category"].ToString()!,
                        IsAlcoholic = (bool)reader["isAlcoholic"],
                        CategoryStatus = ECategoryStatus.pending // Default value when loading
                    };

                    // Create an OrderItem object, linking the loaded MenuItem
                    OrderItem orderItem = new OrderItem
                    {
                        OrderItemId = (int)reader["orderItem_id"],
                        Quantity = (int)reader["quantity"],
                        Note = reader["note"].ToString()!,
                        ItemStatus = Enum.Parse<EItemStatus>(reader["itemStatus"].ToString()!), // Parse enum from string
                        OrderNumber = orderNumber,
                        MenuItem = menuItem
                    };

                    // Add the order item to the list
                    items.Add(orderItem);
                }
            }

            // Return the full list of order items for the specified order
            return items;
        }




        public List<RunningOrderWithItemsViewModel> GetOrdersWithItems(bool IsDrink)
        {
            var orders = new List<RunningOrderWithItemsViewModel>();
         
            using (var connection = new SqlConnection(_connectionString))
            {
                string categoryFilter = IsDrink
                    ? "'drink'"                                   // Ternary Operator 
                    : "'food'";

                string query = @"
                    SELECT 
                         o.orderNumber, o.status, o.tableNumber, o.orderTime, o.finishOrderTime,
                         oi.orderItem_id, oi.quantity, oi.note, oi.itemStatus,
                         mi.menuItem_id, mi.menuItemName, mi.category , mi.categoryStatus
                     FROM ORDERS o
                     JOIN ORDER_ITEM oi ON o.orderNumber = oi.orderNumber
                     JOIN MENU_ITEMS mi ON oi.menuItem_id = mi.menuItem_id
                     WHERE status <> 'onHold' AND oi.itemStatus <> 'onHold' AND " +
                     $"mi.itemType in ({categoryFilter}) "+ 
                     "ORDER BY o.orderNumber, oi.orderItem_id";
               

                var command = new SqlCommand(query, connection);
                command.Connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    int lastOrderNumber = -1;
                    RunningOrderWithItemsViewModel currentOrder = null!;

                    while (reader.Read())
                    {
                        int orderNumber = reader.GetInt32(reader.GetOrdinal("orderNumber"));

                        // اگه وارد سفارش جدید شدیم
                        if (orderNumber != lastOrderNumber)
                        {
                            currentOrder = new RunningOrderWithItemsViewModel
                            {
                                OrderNumber = orderNumber,
                                Status = (EOrderStatus)Enum.Parse(typeof(EOrderStatus) , reader.GetString(reader.GetOrdinal("status"))),
                                TableNumber = reader.GetInt32(reader.GetOrdinal("tableNumber")),
                                OrderTime = reader.GetDateTime(reader.GetOrdinal("orderTime")),
                                FinishOrderTime = reader.IsDBNull(reader.GetOrdinal("finishOrderTime"))
                                    ? (DateTime?)null
                                    : reader.GetDateTime(reader.GetOrdinal("finishOrderTime")),

                                WaitingTime = DateTime.Now - reader.GetDateTime(reader.GetOrdinal("orderTime"))

                            };

                            orders.Add(currentOrder);
                            lastOrderNumber = orderNumber;
                        }

                        // آیتم‌های سفارش را اضافه کن
                        var orderItem = new RunningOrderItemWithMenuItemViewModel
                        {
                            OrderItemId = reader.GetInt32(reader.GetOrdinal("orderItem_id")),
                            Quantity = reader.GetInt32(reader.GetOrdinal("quantity")),
                            Note = reader.IsDBNull(reader.GetOrdinal("note"))
                                    ? (string?)null
                                    : reader.GetString(reader.GetOrdinal("note")),

                            ItemStatus = (EItemStatus)Enum.Parse(typeof(EItemStatus), reader.GetString(reader.GetOrdinal("itemStatus"))),

                            MenuItemId = reader.GetInt32(reader.GetOrdinal("menuItem_id")),
                            MenuItemName = reader.GetString(reader.GetOrdinal("menuItemName")),
                            OrderItemCategory = reader.GetString(reader.GetOrdinal("category")),
                            MenuItemCategoryStatus = (ECategoryStatus)Enum.Parse(typeof(ECategoryStatus), reader.GetString(reader.GetOrdinal("categoryStatus"))),

                        };

                        currentOrder.runningOrderItems.Add(orderItem);
                    }
                }
            }

            return orders;
        }

    }
}
