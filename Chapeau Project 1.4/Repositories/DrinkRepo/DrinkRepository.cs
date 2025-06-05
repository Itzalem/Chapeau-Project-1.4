//using Chapeau_Project_1._4.Models;
//using Microsoft.Data.SqlClient;

//namespace Chapeau_Project_1._4.Repositories.DrinkRepo
//{
//    public class DrinkRepository : IDrinkRepository
//    {
//        private readonly string? _connectionString;

//        public DrinkRepository(IConfiguration configuration)
//        {
//            _connectionString = configuration.GetConnectionString("chapeaurestaurant");
//        }

//        public List<Drink> GetDrinkOrders()
//        {
//            //List<Drink> drinks = new List<Drink>();
//            //using (SqlConnection connection = new SqlConnection(_connectionString))
//            //{
//            //    string query = @"SELECT 
//            //                     o.orderNumber,
//            //                     o.status AS orderStatus,
//            //                     o.tableNumber,
//            //                     o.orderTime,
//            //                     oi.orderItem_id,
//            //                     oi.quantity,
//            //                     oi.note,
//            //                     oi.itemStatus,
//            //                     mi.itemType
//            //                     FROM ORDERS o
//            //                     JOIN ORDER_ITEM oi ON o.orderNumber = oi.orderNumber
//            //                     JOIN MENUITEM mi ON oi.menuItem_id = mi.menuItem_id
//            //                     WHERE mi.itemType = 'drink';"; 
//            //    }

//            throw new NotImplementedException();    
//        }

//        public Drink GetDrinks(int? drinkId)
//        {

//            //the connection string was set in the constructor 
//            using (SqlConnection connection = new SqlConnection(_connectionString))
//            {
//                string query = "SELECT drink_id, drinkName, isAlcoholic, menuItem_id FROM DRINK WHERE @drinkId = drink_id";
//                SqlCommand command = new SqlCommand(query, connection);

//                command.Parameters.AddWithValue("@drinkId", drinkId);

//                command.Connection.Open();
//                SqlDataReader reader = command.ExecuteReader();

//                Drink drink = ReadDrink(reader);

//                return drink;
//            }
//        }

//        public List<Drink> GetFinishedDrinksOrder()
//        {
//            throw new NotImplementedException();
//        }

//        private Drink ReadDrink(SqlDataReader reader)
//        {
//            int DrinkId = (int)reader["drink_id"];
//            string DrinkName = (string)reader["drinkName"];
//            bool IsAlcoholic = (bool)reader["isAlcoholic"];
//            int MenuItemId = (int)reader["menuItem_id"];

//            return new Drink(DrinkId, DrinkName, IsAlcoholic, MenuItemId);
//        }
//    }
//}
