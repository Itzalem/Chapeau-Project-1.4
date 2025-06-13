using Chapeau_Project_1._4.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Chapeau_Project_1._4.Repositories.PaymentRepo
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly string? _connectionString;

        public PaymentRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ChapeauRestaurant");
        }

        public void CreateBill(Payment payment)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO BILL (tip, orderNumber, tableNumber)" +
                " VALUES (@Tip, @OrderNumber, @TableNumber)";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Tip", payment.Tip);
                command.Parameters.AddWithValue("@OrderNumber", payment.Bill.Order.OrderNumber);
                command.Parameters.AddWithValue("@tableNumber", payment.Bill.Table.TableNumber);

				command.Connection.Open();

				int rowsChanged = command.ExecuteNonQuery();
                if (rowsChanged != 1)
                {
                    throw new Exception("Item addition failed");
                }
            }
        }

		public void CreatePayment(Payment payment)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				string query = "INSERT INTO PAYMENT (bill_id, tip, total, payMethod, feedback, VAT)" +
				" VALUES (@BillId, @Tip, @Total, @PaymentType, @Feedback, @VAT)";

				SqlCommand command = new SqlCommand(query, connection);

				command.Parameters.AddWithValue("@BillId", payment.Bill.BillId);
				command.Parameters.AddWithValue("@Tip", payment.Tip);
				command.Parameters.AddWithValue("@Total", payment.Total);
				command.Parameters.AddWithValue("@PaymentType", payment.PaymentType);
				command.Parameters.AddWithValue("@Feedback", payment.Feedback);
				command.Parameters.AddWithValue("@VAT", payment.VAT);

				command.Connection.Open();

				int rowsChanged = command.ExecuteNonQuery();
				if (rowsChanged != 1)
				{
					throw new Exception("Item addition failed");
				}
			}
		}

		private Bill ReadBill(SqlDataReader reader, Payment payment)
		{
			int BillId = (int)reader["bill_id"];
			decimal Tip = (decimal)reader["tip"];

			return new Bill(BillId, Tip, payment.Bill.Order, payment.Bill.Table);
		}

		public Bill GetBill(Payment payment)
		{
			Bill bill = new Bill();

			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				string query = @"SELECT bill_id, tip 
									FROM BILL 
									WHERE orderNumber = @OrderNumber ;";

				SqlCommand command = new SqlCommand(query, connection);

				command.Parameters.AddWithValue("@OrderNumber", payment.Bill.Order.OrderNumber);

				command.Connection.Open();
				SqlDataReader reader = command.ExecuteReader();

				if (reader.Read())
				{
					bill = ReadBill(reader, payment);
				}
				reader.Close();
			}

			return bill;
		}

		public void UpdateOrderStatus(Payment payment)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				string query = " UPDATE ORDERS SET status = @Status " +
								"WHERE orderNumber = @OrderNumber;";

				SqlCommand command = new SqlCommand(query, connection);

				command.Parameters.AddWithValue("@Status", EOrderStatus.paid.ToString());
				command.Parameters.AddWithValue("@OrderNumber", payment.Bill.Order.OrderNumber);

				command.Connection.Open();

				int rows = command.ExecuteNonQuery();
				if (rows != 1)
				{
					throw new Exception("Failed to reduce stock");
				}
			}
		}

		public void UpdateTableStatus(Payment payment)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				string query = " UPDATE RESTAURANT_TABLE SET isOccupied = 0 " +
								"WHERE tableNumber = @TableNumber;";

				SqlCommand command = new SqlCommand(query, connection);

				command.Parameters.AddWithValue("@TableNumber", payment.Bill.Table.TableNumber);

				command.Connection.Open();

				int rows = command.ExecuteNonQuery();
				if (rows != 1)
				{
					throw new Exception("Failed to reduce stock");
				}
			}
		}
	}
}
