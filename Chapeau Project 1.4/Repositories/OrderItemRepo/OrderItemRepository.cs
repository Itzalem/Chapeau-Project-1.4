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
                string query = "INSERT INTO ORDER_ITEM (quantity, note, menuItem_id, orderNumber, itemStatus)" + 
                                "VALUES (@Quantity, @Note, @MenuItemId, @OrderNumber, @ItemStatus);";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Quantity", orderItem.Quantity);

                if (orderItem.Note == null)
                    command.Parameters.AddWithValue("@Note", DBNull.Value);
                else
                    command.Parameters.AddWithValue("@Note", orderItem.Note);

                command.Parameters.AddWithValue("@MenuItemId", orderItem.MenuItem.MenuItemId);
                command.Parameters.AddWithValue("@OrderNumber", orderItem.OrderNumber);
                command.Parameters.AddWithValue("@ItemStatus", orderItem.ItemStatus);

                command.Connection.Open();

                int rowsChanged = command.ExecuteNonQuery();
                if (rowsChanged != 1)
                {
                    throw new Exception("Item addition failed");
                }
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
                                    FROM ORDER_ITEM  WHERE orderNumber = @orderNumber ;" ;


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

        public void UpdateCourseStatus(int orderNumber , EItemStatus newStatus)
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

    }
}
