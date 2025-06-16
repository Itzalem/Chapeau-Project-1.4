using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.Repositories.MenuRepo;
using Chapeau_Project_1._4.ViewModel;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Reflection.PortableExecutable;

namespace Chapeau_Project_1._4.Repositories.OrderItemRepo
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly string? _connectionString;
        private readonly IMenuRepository _menuRepository;

        public OrderItemRepository(IConfiguration configuration, IMenuRepository menuRepository)
        {
            // get (database connectionstring from appsetings) 
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

            return orderItem;
        }

        public List<OrderItem> DisplayItemsPerOrder(Order order) 
        {
            List<OrderItem> orderItems = new List<OrderItem>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT orderItem_id, quantity, note, menuItem_id, orderNumber, itemStatus 
                                    FROM ORDER_ITEM  WHERE orderNumber = @orderNumber ;";


                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@orderNumber", order.OrderNumber);

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

        public void InsertOrderItem(OrderItem orderItem) //creates a new orderItem in db
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO ORDER_ITEM (quantity, note, menuItem_id, orderNumber, itemStatus)" +
                            " VALUES (@Quantity, @Note, @MenuItemId, @OrderNumber, @ItemStatus)";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Quantity", orderItem.Quantity);
                command.Parameters.AddWithValue("@Note", (object?)orderItem.Note ?? DBNull.Value);
                command.Parameters.AddWithValue("@MenuItemId", orderItem.MenuItem.MenuItemId);
                command.Parameters.AddWithValue("@OrderNumber", orderItem.OrderNumber);
                command.Parameters.AddWithValue("@ItemStatus", orderItem.ItemStatus.ToString());

                connection.Open();

                int rowsChanged = command.ExecuteNonQuery();
                if (rowsChanged != 1)
                {
                    throw new Exception("Item addition failed");
                }
            }

        }

        public bool CheckDuplicateItems(OrderItem orderItem)
        {
            int? existingItemId = FindMatchingOrderItem(orderItem);

            if (existingItemId != null && existingItemId != orderItem.OrderItemId)
            {
                UpdateQuantity(existingItemId.Value, orderItem.Quantity);
                DeleteDuplicateItem(orderItem.OrderItemId); // delete the new duplicate, keep the existing one
                return true;
            }
            else
            {
                return false;
            }
        }

        private int? FindMatchingOrderItem(OrderItem orderItem) 
            // Check if the item already exists in the order with same parameters
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT orderItem_id FROM ORDER_ITEM " +
                           " WHERE orderNumber = @OrderNumber AND menuItem_id = @MenuItemId " +
                           " AND ((note IS NULL AND @Note IS NULL) OR note = @Note)  " +
                           " AND itemStatus = @OrderItemStatus " +
                           " AND orderItem_id<> @CurrentItemId ;"; //if its in a loop it will exclude itself

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@OrderNumber", orderItem.OrderNumber);
                command.Parameters.AddWithValue("@MenuItemId", orderItem.MenuItem.MenuItemId);
                //value note normalization 
                command.Parameters.AddWithValue("@Note", string.IsNullOrWhiteSpace(orderItem.Note) ? DBNull.Value : orderItem.Note);
                command.Parameters.AddWithValue("@OrderItemStatus", orderItem.ItemStatus.ToString());
                command.Parameters.AddWithValue("@CurrentItemId", orderItem.OrderItemId);

                connection.Open();

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
        }

        public void UpdateQuantity(int existingItemId, int extraQuantity) 
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "UPDATE ORDER_ITEM SET quantity = quantity + @ExtraQuantity " +
                            " WHERE orderItem_id = @Id ;";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@ExtraQuantity", extraQuantity);
                command.Parameters.AddWithValue("@Id", existingItemId);

                connection.Open();
                int rowsChanged = command.ExecuteNonQuery();
                if (rowsChanged != 1)
                {
                    throw new Exception("Failed to update duplicate item quantity");
                }
            }
        }

        public void DeleteDuplicateItem(int duplicateItemId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM ORDER_ITEM WHERE orderItem_id = @ExistingId ";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@ExistingId", duplicateItemId);

                connection.Open();
                int rowsChanged = command.ExecuteNonQuery();
                if (rowsChanged != 1)
                {
                    throw new Exception("Failed to delete duplicate item");
                }
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

        public void UpdateHoldItemsStatus(Order order) //hold items are updated to pending after send
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = " UPDATE ORDER_ITEM SET itemStatus = @UpdatedItemStatus " +
                                    "WHERE orderNumber = @OrderNumber AND itemStatus = @HoldStatus;";

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@UpdatedItemStatus", EItemStatus.pending.ToString());
                    command.Parameters.AddWithValue("@OrderNumber", order.OrderNumber);
                    command.Parameters.AddWithValue("@HoldStatus", EItemStatus.onHold.ToString());

                    command.Connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating items status", ex);
            }
        }

        public OrderItem GetOrderItemById(int orderItemId) 
        {
            OrderItem orderItem = new OrderItem();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT orderItem_id, quantity, note, menuItem_id, orderNumber, itemStatus 
                                    FROM ORDER_ITEM  WHERE orderItem_id = @OrderItemId ;";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@OrderItemId", orderItemId);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    orderItem = ReadOrderItem(reader);
                }
                reader.Close();
            }

            return orderItem;

        }

        public void EditItemQuantity(OrderItem orderItem)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = " UPDATE ORDER_ITEM SET quantity = @UpdatedQuantity " +
                                "WHERE orderItem_id = @OrderItem_Id;";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@UpdatedQuantity", orderItem.Quantity);
                command.Parameters.AddWithValue("@OrderItem_Id", orderItem.OrderItemId);

                command.Connection.Open();

                int rows = command.ExecuteNonQuery();
                if (rows != 1)
                {
                    throw new Exception("Failed to update quantity");
                }
            }
        }

        public void DeleteSingleItem(OrderItem orderItem)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"DELETE FROM ORDER_ITEM 
                                 WHERE orderItem_id = @OrderItem_Id";


                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@OrderItem_Id", orderItem.OrderItemId);

                command.Connection.Open();
                int rows = command.ExecuteNonQuery();
                if (rows != 1)
                {
                    throw new Exception("Failed to delete item");
                }

            }
        }

        public void EditItemNote(OrderItem orderItem)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = " UPDATE ORDER_ITEM SET note = @Note " +
                                "WHERE orderItem_id = @OrderItem_Id;";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Note", orderItem.Note);
                command.Parameters.AddWithValue("@OrderItem_Id", orderItem.OrderItemId);

                command.Connection.Open();
                int rows = command.ExecuteNonQuery();
                if (rows != 1)
                {
                    throw new Exception("Failed to reduce stock");
                }
            }

        }

        ////////////

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

        public void UpdateCourseStatus(int orderNumber, string category, ECategoryStatus categoryCourseStatus)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {

                // پیدا کردن MenuItemId های مربوط به سفارش و دسته مورد نظر
                string query = @"
                                    SELECT DISTINCT mi.menuItem_id,oi.orderNumber
                                    FROM ORDER_ITEM oi
                                    JOIN MENU_ITEMS mi ON oi.menuItem_id = mi.menuItem_id
                                    WHERE oi.orderNumber = @orderNumber AND mi.category = @category" ;


                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@orderNumber", orderNumber);
                command.Parameters.AddWithValue("@category", category);

                command.Connection.Open();

                var menuItemIds = new List<int>();
                int orderNumberId = 0;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        menuItemIds.Add(reader.GetInt32(0));
                        orderNumberId = reader.GetInt32(1);
                    }
                }

                // اگر آیتمی پیدا شد، categoryStatus مربوطه را آپدیت کن
                foreach (var menuItemId in menuItemIds)
                {
                    if (orderNumber == orderNumberId)
                    {
                        string queryUpdate = @"
                            UPDATE MENU_ITEMS
                            SET categoryStatus = @groupStatus
                            WHERE menuItem_id = @menuItemId
                        ";
                        SqlCommand updateCmd = new SqlCommand(queryUpdate, connection);

                        updateCmd.Parameters.AddWithValue("@groupStatus", categoryCourseStatus.ToString());
                        updateCmd.Parameters.AddWithValue("@menuItemId", menuItemId);

                        updateCmd.ExecuteNonQuery();
                    }

                }

                connection.Close();
            }
        }

        
        //Lukas
        public List<OrderItem> GetOrderItemsForServing(int orderNumber)
        {
            List<OrderItem> orderItems = new List<OrderItem>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT 
                            oi.orderItem_id,
                            oi.orderNumber,
                            oi.menuItem_id AS oi_menuItem_id,
                            oi.quantity,
                            oi.note,
                            oi.itemStatus
                         FROM ORDER_ITEM oi
                         WHERE oi.orderNumber = @orderNumber;";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@orderNumber", orderNumber);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    //implementing all this to not intefere with exisiting code because it did not work...
                    int orderItemId = (int)reader["orderItem_id"];
                    int quantity = (int)reader["quantity"];
                    string note = reader["note"] == DBNull.Value ? "" : (string)reader["note"];
                    int menuItemId = (int)reader["oi_menuItem_id"];
                    EItemStatus itemStatus = (EItemStatus)Enum.Parse(typeof(EItemStatus), reader["itemStatus"].ToString()!);
                    int orderNum = (int)reader["orderNumber"];

                    MenuItem menuItem = _menuRepository.GetMenuItemById(menuItemId);

                    var orderItem = new OrderItem(orderItemId, quantity, note, itemStatus, menuItem, orderNum);
                    orderItems.Add(orderItem);
                }

                reader.Close();
            }

            return orderItems;
        }


    }
}
