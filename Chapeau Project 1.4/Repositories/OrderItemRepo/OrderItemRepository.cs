using Chapeau_Project_1._4.Models;
using Microsoft.Data.SqlClient;

namespace Chapeau_Project_1._4.Repositories.OrderItemRepo
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly string? _connectionString;

        public OrderItemRepository(IConfiguration configuration)
        {
            // get (database connectionstring from appsetings 
            _connectionString = configuration.GetConnectionString("ChapeauRestaurant");
        }

        private OrderItem ReadOrderItem(SqlDataReader reader)
        {
            int OrderNumber = (int)reader["orderNumber"];
            EItemStatus itemStatus = (EItemStatus)Enum.Parse(typeof(EItemStatus), reader["itemStatus"].ToString()!);
            int OrderItemId = (int)reader["orderItem_id"];
            int quantity = (int)reader["quantity"];
            int MenuItemName = (int)reader["menuItemName"];
            string note = (string)reader["note"];


            return new OrderItem(
            
                OrderItemId,
                quantity,
                note,
                itemStatus,
                MenuItemName,
                OrderNumber
               
            );
        }
        public List<OrderItem> DisplayOrderItem()
        {
            List<OrderItem> orderItems = new List<OrderItem>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT orderItem_id, quantity, note, menuItemName, orderNumber, itemStatus
                                FROM ORDER_ITEM
                                INNER JOIN MENU_ITEMS
                                ON ORDER_ITEM.menuItem_id = MENU_ITEMS.menuItem_id";
                SqlCommand command = new SqlCommand(query, connection);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    OrderItem orderItem = ReadOrderItem(reader);
                    orderItems.Add(orderItem);
                }
                reader.Close();

            }
            return orderItems;
        }

        public List<OrderItem> GetByOrderNumber(int orderNumber)
        {
            List<OrderItem > orderItems = new List<OrderItem>();    

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @" SELECT orderItem_id, orderNumber, menuItem_id, quantity, note, itemStatus, menuItemName, category
                       FROM ORDER_ITEM
                       JOIN MENU_ITEMS ON ORDER_ITEM.menuItem_id = MENU_ITEMS.menuItem_id
                       WHERE ORDER_ITEM.orderNumber = @orderNumber
                       ORDER BY MENU_ITEMS.category, ORDER_ITEM.orderItem_id;";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@orderNumber", orderNumber);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    OrderItem orderItem = ReadOrderItem(reader);
                    orderItems.Add(orderItem);
                }
                reader.Close();

            }
            return orderItems;
        }
        

        public List<OrderItem> GetRunningItem()
        {
            List<OrderItem> orderItems = new List<OrderItem>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @" SELECT orderItem_id, orderNumber, menuItem_id, quantity, note, itemStatus
                                  FROM ORDER_ITEM
                                  WHERE itemStatus <> @ready
                                  ORDER BY orderNumber, orderItem_id;";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ready", EItemStatus.ReadyToServe.ToString());

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    OrderItem orderItem = ReadOrderItem(reader);
                    orderItems.Add(orderItem);
                }
                reader.Close();
            }

            return orderItems ; 
        }

        public void UpdateItemStatus(int orderItemId, EItemStatus newStatus)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @" UPDATE ORDER_ITEM 
                                  SET itemStatus = @st
                                  WHERE orderItem_id = @id;";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@st", newStatus.ToString());
                command.Parameters.AddWithValue("@id", orderItemId);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();
            }
        }

        public void UpdateCourseStatus(int orderNumber, string courseCategory, EItemStatus newStatus)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @" UPDATE ORDER_ITEM
                                  SET itemStatus = @st
                                  FROM ORDER_ITEM
                                  JOIN MENU_ITEMS ON ORDER_ITEM.menuItem_id = MENU_ITEMS.menuItem_id
                                  WHERE ORDER_ITEM.orderNumber = @ord
                                  AND MENU_ITEMS.category = @cat;";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@st", newStatus.ToString());
                command.Parameters.AddWithValue("@ord", orderNumber);
                command.Parameters.AddWithValue("@cat", courseCategory);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();
            }
        }

        public List<OrderItem> GetFinishedItems(DateTime date)
        {
            List<OrderItem> orderItems = new List<OrderItem>(); 


            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @" SELECT orderItem_id, orderNumber, menuItem_id, quantity, note, itemStatus
                                  FROM ORDER_ITEM
                                  JOIN ORDERS ON ORDER_ITEM.orderNumber = ORDERS.orderNumber
                                  WHERE ORDER_ITEM.itemStatus = @ready
                                  AND CAST(ORDERS.orderTime AS DATE) = @dt;";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ready", EItemStatus.ReadyToServe.ToString());
                command.Parameters.AddWithValue("@dt", date.Date);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    OrderItem orderItem = ReadOrderItem(reader);
                    orderItems.Add(orderItem);
                }
                reader.Close();
            }
            return orderItems; 
        }
    }
}
