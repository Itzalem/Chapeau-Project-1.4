using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.Repositories.MenuRepo;
using Microsoft.Data.SqlClient;

namespace Chapeau_Project_1._4.Repositories.OrderItemRepo
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly string? _connectionString;
        private readonly IMenuRepository _menuRepository;

        public OrderItemRepository(IConfiguration configuration, IMenuRepository menuRepository)
        {
            // get (database connectionstring from appsetings 
            _connectionString = configuration.GetConnectionString("ChapeauRestaurant");
            _menuRepository = menuRepository;
        }

        private OrderItem ReadOrderItem(SqlDataReader reader)
        {
            int orderNumber = (int)reader["orderNumber"];
            EItemStatus itemStatus = (EItemStatus)Enum.Parse(typeof(EItemStatus), reader["itemStatus"].ToString()!);
            int orderItemId = (int)reader["orderItem_id"];
            int quantity = (int)reader["quantity"];
            string note = reader["note"] == DBNull.Value ? "" : (string)reader["note"];
            int menuItemId = (int)reader["menuItem_id"];


            MenuItem menuItem = _menuRepository.GetMenuItemById(menuItemId);

            var orderItem = new OrderItem(

                orderItemId,
                quantity,
                note,
                itemStatus,
                menuItem,
                orderNumber
            );


            //orderItem.MenuItem = new MenuItem(menuItemId, menuItemName, category, categoryStatus);

            return orderItem;
        }

        public void AddOrderItem(OrderItem orderItem)
        {

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                int? existingItemId = FindMatchingOrderItemId(connection, orderItem);

                if (existingItemId != null)
                {
                    UpdateQuantity(connection, existingItemId.Value, orderItem.Quantity);
                }
                else
                {
                    InsertOrderItem(connection, orderItem);
                }
            }
        }

        private int? FindMatchingOrderItemId(SqlConnection connection, OrderItem orderItem)
        {
            string query = "SELECT orderItem_id FROM ORDER_ITEM " +
                           " WHERE orderNumber = @OrderNumber AND menuItem_id = @MenuItemId " +
                           " AND ((note IS NULL AND @Note IS NULL) OR note = @Note)";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@OrderNumber", orderItem.OrderNumber);
            command.Parameters.AddWithValue("@MenuItemId", orderItem.MenuItem.MenuItemId);
            command.Parameters.AddWithValue("@Note", (object?)orderItem.Note ?? DBNull.Value);

            object? result = command.ExecuteScalar();

            if (result != null)
            {
                return (int?)Convert.ToInt32(result);
            }
            else
            {
                return null;
            }

        }

        private void UpdateQuantity(SqlConnection connection, int orderItemId, int extraQuantity)
        {
            string query = "UPDATE ORDER_ITEM SET quantity = quantity + @ExtraQuantity " +
                            " WHERE orderItem_id = @Id ;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ExtraQuantity", extraQuantity);
            command.Parameters.AddWithValue("@Id", orderItemId);
            command.ExecuteNonQuery();
        }

        private void InsertOrderItem(SqlConnection connection, OrderItem orderItem)
        {
            string query = "INSERT INTO ORDER_ITEM (quantity, note, menuItem_id, orderNumber, itemStatus)" +
                            " VALUES (@Quantity, @Note, @MenuItemId, @OrderNumber, @ItemStatus)";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Quantity", orderItem.Quantity);
            command.Parameters.AddWithValue("@Note", (object?)orderItem.Note ?? DBNull.Value);
            command.Parameters.AddWithValue("@MenuItemId", orderItem.MenuItem.MenuItemId);
            command.Parameters.AddWithValue("@OrderNumber", orderItem.OrderNumber);
            command.Parameters.AddWithValue("@ItemStatus", orderItem.ItemStatus.ToString());

            int rowsChanged = command.ExecuteNonQuery();
            if (rowsChanged != 1)
            {
                throw new Exception("Item addition failed");
            }

        }

        public List<OrderItem> DisplayOrderItems()
        {
            List<OrderItem> orderItems = new List<OrderItem>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT orderItem_id ,MNT.menuItem_id,MNT.menuItemName , MNT.category , MNT.categoryStatus, quantity, note, menuItemName, orderNumber, itemStatus
                                    FROM ORDER_ITEM
                                    INNER JOIN MENU_ITEMS as MNT
                                    ON ORDER_ITEM.menuItem_id = MNT.menuItem_id
                                    WHERE MNT.category in ('Starters','Mains','Desserts') 
                                    AND ORDER_ITEM.itemStatus <> 'onHold'
                                    ORDER By MNT.category desc";
                // in the where i need another filtering that states itemstatus is different in onhold so i will not catch the onhold orders 

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


        public List<OrderItem> DisplayItemsPerOrder(int orderNumber)
        {
            List<OrderItem> orderItems = new List<OrderItem>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT orderItem_id, quantity, note, menuItem_id, orderNumber, itemStatus 
                                    FROM ORDER_ITEM  WHERE orderNumber = @orderNumber ;";


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


        public List<OrderItem> GetByOrderNumber(int orderNumber)
        {
            List<OrderItem> orderItems = new List<OrderItem>();

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

            return orderItems;
        }

        public void UpdateAllItemsStatus(EItemStatus updatedItemStatus, Order order)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = " UPDATE ORDER_ITEM SET itemStatus = @UpdatedItemStatus " +
                                "WHERE orderNumber = @OrderNumber;";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@UpdatedItemStatus", updatedItemStatus.ToString());
                command.Parameters.AddWithValue("@OrderNumber", order.OrderNumber);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();
            }

        }

        public void ReduceItemStock(OrderItem orderItem)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = " UPDATE MENU_ITEMS SET stock = stock - @UpdatedStock " +
                                "WHERE menuItem_id = @MenuItem_Id;";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@UpdatedStock", orderItem.Quantity);
                command.Parameters.AddWithValue("@MenuItem_Id", orderItem.MenuItem.MenuItemId);

                command.Connection.Open();
                int rows = command.ExecuteNonQuery();
                if (rows != 1)
                {
                    throw new Exception("Failed to reduce stock");
                }
            }
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

        public void UpdateCourseStatus(int orderNumber, EItemStatus newStatus)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @" UPDATE ORDER_ITEM
                                  SET itemStatus = @st
                                  FROM ORDER_ITEM
                                  JOIN MENU_ITEMS ON ORDER_ITEM.menuItem_id = MENU_ITEMS.menuItem_id
                                  WHERE ORDER_ITEM.orderNumber = @ord;";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@st", newStatus.ToString());
                command.Parameters.AddWithValue("@ord", orderNumber);

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

        public List<OrderItem> DisplayItemsPerOrder(int orderNumber)
        {
            throw new NotImplementedException();
        }
    }
}
