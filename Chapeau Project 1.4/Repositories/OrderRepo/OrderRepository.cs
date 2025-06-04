using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.Services.OrderItems;
using Chapeau_Project_1._4.ViewModel;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace Chapeau_Project_1._4.Repositories.OrderRepo
{
    public class OrderRepository : IOrderRepository
    {

        private readonly string? _connectionString;
        private readonly IOrderItemService _orderItemService;
        

        public OrderRepository(IConfiguration configuration, IOrderItemService orderItemService)
        {
            // get (database connectionstring from appsetings 
            _connectionString = configuration.GetConnectionString("ChapeauRestaurant");
            _orderItemService = orderItemService;
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
                string query = @"SELECT orderNumber, status, tableNumber, orderTime 
                                 FROM ORDERS
                                 WHERE status <> 'onHold'";
                SqlCommand command = new SqlCommand(query, connection);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Order order = ReadOrder(reader);
                    orders.Add(order);
                }
                reader.Close();

            }
            return orders;
        }

        public int AddNewOrder(int tableNumber)  
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
                                 AND status IN ('pending', 'inProgress', 'prepared')";
                                

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
         
        /* COMMENTED BECAUSE IT DOESN'T WORK, THIJMEN CHECK THIS PLEASE
        private Order ReadOrderByTable(SqlDataReader reader)
        {
            int OrderNumber = (int)reader["orderNumber"];
            EOrderStatus Status = (EOrderStatus)Enum.Parse(typeof(EOrderStatus), reader["status"].ToString()!, true);
            DateTime OrderTime = (DateTime)reader["orderTime"];
            int TableNumber = (int)reader["tableNumber"];

            List<OrderItem> orderItems = _orderItemService.DisplayItemsPerOrder(OrderNumber);

            return new Order(OrderNumber, Status, OrderTime, TableNumber, orderItems);
        }*/

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

        public void UpdateOrderStatus(EOrderStatus status, int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE Orders SET status = @status WHERE orderNumber = @id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@status", status.ToString());
                command.Parameters.AddWithValue("@id", id);

                command.Connection.Open() ; 
                command.ExecuteNonQuery();
            }
        }

        public void CancelUnsentOrder(Order order)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"DELETE FROM ORDER_ITEM 
                                 WHERE itemStatus = @Status and orderNumber = @OrderNumber ";


                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Status", EItemStatus.onHold.ToString());
                command.Parameters.AddWithValue("@OrderNumber", order.OrderNumber);

                command.Connection.Open();
                command.ExecuteNonQuery();

                int itemsInList = CountItemsInOrder(order);

                if (itemsInList == 0)
                {
                    DeleteEmptyOrder(order);
                }
            }
        }      


        public void DeleteEmptyOrder(Order order)
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
            return null;

        }

        private object ReadOrderGraph(SqlDataReader reader)
        {
            int orderItemId = (int)reader["orderItem_id"];
            int menuItemId = (int)reader["menuItem_id"];
            string menuItemName = (string)reader["menuItemName"];
            string category = (string)reader["category"];
            ECategoryStatus categoryStatus = (ECategoryStatus)Enum.Parse(typeof(ECategoryStatus), reader["categoryStatus"].ToString()!);

            return new{ OrderItemId  = orderItemId  , MenuItemId = menuItemId , Category = category , CategoryStatus = categoryStatus};
        }


        //for the overview - Lukas
        public List<OrderItem> GetOrderItemsByOrderNumber(int orderNumber)
        {
            List<OrderItem> items = new List<OrderItem>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"SELECT oi.orderItem_id, oi.itemStatus,
                         mi.menuItem_id, mi.menuItemName, mi.category
                         FROM ORDER_ITEM oi
                         INNER JOIN MENU_ITEMS mi ON oi.menuItem_id = mi.menuItem_id
                         WHERE oi.orderNumber = @orderNumber";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@orderNumber", orderNumber);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    OrderItem item = new OrderItem
                    {
                        OrderItemId = (int)reader["orderItem_id"],
                        ItemStatus = Enum.Parse<EItemStatus>(reader["itemStatus"].ToString()!),
                        MenuItem = new MenuItem
                        {
                            MenuItemId = (int)reader["menuItem_id"],
                            MenuItemName = reader["menuItemName"].ToString()!,
                            Category = reader["category"].ToString()!, // keep string for category
                            CategoryStatus = ECategoryStatus.pending  // default
                        }
                    };
                    items.Add(item);
                }
            }

            return items;
        }







    }
}
