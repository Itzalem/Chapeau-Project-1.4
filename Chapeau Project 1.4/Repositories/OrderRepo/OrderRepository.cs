using Chapeau_Project_1._4.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace Chapeau_Project_1._4.Repositories.OrderRepo
{
    public class OrderRepository : IOrderRepository
    {

        private readonly string? _connectionString;

        public OrderRepository(IConfiguration configuration)
        {
            // get (database connectionstring from appsetings 
            _connectionString = configuration.GetConnectionString("ChapeauRestaurant");
        }


        private Order ReadOrder(SqlDataReader reader)
        {
            int OrderNumber = (int)reader["orderNumber"];
            EOrderStatus Status =(EOrderStatus)Enum.Parse(typeof(EOrderStatus) ,reader["status"].ToString()!);
            DateTime OrderTime = (DateTime)reader["orderTime"];
            int TableNumber = (int)reader["tableNumber"];
           
            return new Order(OrderNumber, Status, OrderTime, TableNumber);
        }

        public List<Order> DiplayOrder()
        {
            
            List<Order> orders = new List<Order>(); 

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT orderNumber, status, tableNumber, orderTime FROM ORDERS";
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

                command.Parameters.AddWithValue("@Status", EOrderStatus.pending);
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

        public Order? GetOrderById(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query =@"SELECT orderNumber, status, tableNumber, orderTim FROM Orders WHERE orderNumber = @id";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);

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
                string query = @"SELECT orderNumber, status, tableNumber, orderTim FROM Orders WHERE tableNumber = @table";

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
    }
}
