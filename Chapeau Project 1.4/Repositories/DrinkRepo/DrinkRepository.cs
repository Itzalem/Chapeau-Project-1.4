using Chapeau_Project_1._4.Models;
using Chapeau_Project_1._4.ViewModel;
using Microsoft.Data.SqlClient;

namespace Chapeau_Project_1._4.Repositories.DrinkRepo
{
    public class DrinkRepository : IDrinkRepository
    {
        private readonly string? _connectionString;

        public DrinkRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("chapeaurestaurant");
        }

        //public List<RunningOrder> GetDrinkOrders()
        //{
        //    var drinkQueryResult = new List<RunningOrder>();
        //    using (SqlConnection connection = new SqlConnection(_connectionString))
        //    {
        //        // tuye query. tuye where drink hayii ke dakhele DRINK hastan 
        //        string query = @"SELECT 
        //                            o.orderNumber,o.orderTime,o.tableNumber,o.status,
        //                            oi.orderItem_id,oi.quantity, oi.note,oi.itemStatus,
        //                            mi.menuItemName
        //                            FROM ORDERS o
        //                            INNER JOIN ORDER_ITEM oi ON o.orderNumber = oi.orderNumber
        //                            INNER JOIN MENU_ITEMS mi ON oi.menuItem_id = mi.menuItem_id
	       //                         WHERE oi.menuItem_id IN (SELECT menuItem_id FROM DRINK)  
        //                            ORDER BY o.orderTime ASC";

        //        SqlCommand command = new SqlCommand(query, connection);
        //        command.Connection.Open();

        //        using (var reader = command.ExecuteReader())
        //        {
        //            // dictionory baraye modiriate kari, key mishe adad ke ordernumber dakhelesh mirizim, value mishe az jense order 
        //            Dictionary<int, RunningOrder> orderDict = new();
        //            while (reader.Read())
        //            {
        //                // ordernumber ro az sefr begir, check kon, age dakhele dictionory khooneyii ba kilide morede nazar nabud => bia oon khoone ro besaz va in maghadir ro besaz 
        //                int orderNumber = reader.GetInt32(0);
        //                if (!orderDict.ContainsKey(orderNumber))
        //                {
        //                    orderDict[orderNumber] = new RunningOrder
        //                    {
        //                        OrderNumber = orderNumber,
        //                        OrderTime = reader.GetDateTime(1),
        //                        TableNumber = reader.GetInt32(2),
        //                        Status = (EOrderStatus)Enum.Parse(typeof(EOrderStatus), reader.GetString(3), true),
        //                        WaitingTime = DateTime.Now - reader.GetDateTime(1)
        //                    };
        //                }
        //                // hala age oon kilide bood bia ye orderItem besaz 
        //                var orderItem = new RunningOrderItem
        //                {
        //                    OrderItemId = reader.GetInt32(4),
        //                    Quantity = reader.GetInt32(5),
        //                    Note = reader.IsDBNull(6) ? null : reader.GetString(6),
        //                    ItemStatus = (EItemStatus)Enum.Parse(typeof(EItemStatus), reader.GetString(7)),
        //                    RunnigOrderMenuItem = new RunnigOrderMenuItem
        //                    {
        //                        OrderItemName = reader.GetString(8),
        //                    }
        //                };
        //                orderDict[orderNumber].runningOrders.Add(orderItem);
        //            }
        //            drinkQueryResult = orderDict.Values.ToList();
        //        }



        //        //reader.Close();
        //    }

        //    return drinkQueryResult;

        //}

        public Drink GetDrinks(int? drinkId)
        {

            //the connection string was set in the constructor 
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT drink_id, drinkName, isAlcoholic, menuItem_id FROM DRINK WHERE @drinkId = drink_id";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@drinkId", drinkId);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                Drink drink = ReadDrink(reader);

                return drink;
            }
        }

        public List<Drink> GetFinishedDrinksOrder()
        {
            throw new NotImplementedException();
        }

        private Drink ReadDrink(SqlDataReader reader)
        {
            int DrinkId = (int)reader["drink_id"];
            string DrinkName = (string)reader["drinkName"];
            bool IsAlcoholic = (bool)reader["isAlcoholic"];
            int MenuItemId = (int)reader["menuItem_id"];

            return new Drink(DrinkId, DrinkName, IsAlcoholic, MenuItemId);
        }
    }
}
    



