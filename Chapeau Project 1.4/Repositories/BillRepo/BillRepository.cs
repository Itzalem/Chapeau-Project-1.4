using Chapeau_Project_1._4.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Chapeau_Project_1._4.Repositories.BillRepo
{
    public class BillRepository : IBillRepository
    {
        private readonly string? _connectionString;

        public BillRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ChapeauRestaurant");
        }

        public Bill CreateBill(Payment payment)
        {
            //using (SqlConnection connection = new SqlConnection(_connectionString))
            //{
            //    string query = "INSERT INTO ORDER_ITEM (quantity, note, menuItem_id, orderNumber, itemStatus)" +
            //    " VALUES (@Quantity, @Note, @MenuItemId, @OrderNumber, @ItemStatus)";

            //    SqlCommand command = new SqlCommand(query, connection);

            //    //command.Parameters.AddWithValue("@Quantity", orderItem.Quantity);
            //    //command.Parameters.AddWithValue("@Note", (object?)orderItem.Note ?? DBNull.Value);
            //    //command.Parameters.AddWithValue("@MenuItemId", orderItem.MenuItem.MenuItemId);
            //    //command.Parameters.AddWithValue("@OrderNumber", orderItem.OrderNumber);
            //    //command.Parameters.AddWithValue("@ItemStatus", orderItem.ItemStatus.ToString());

            //    int rowsChanged = command.ExecuteNonQuery();
            //    if (rowsChanged != 1)
            //    {
            //        throw new Exception("Item addition failed");
            //    }
            //}
            return new Bill();
        }
    }
}
