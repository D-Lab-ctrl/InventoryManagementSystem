using System;
using MySql.Data.MySqlClient;
class RepositoryOrder
{
    private string _connectionString = "server=127.0.0.1;port=3306;username=root;password=las;database=Company";
    public void Add(Order order)
    {
        try
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                string query = "INSERT INTO Orders(CustomerId, OrderDate, Total) VALUES (@CustomerId, @OrderDate, @Total)";
                MySqlCommand cmd = new MySqlCommand(query, connection);

                connection.Open();

                cmd.Parameters.AddWithValue("@CustomerId", order.CustomerId);
                cmd.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                cmd.Parameters.AddWithValue("@Total", order.Total);

                cmd.ExecuteNonQuery();
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    public List<Order> GetAll()
    {
        List<Order> OrderList = new List<Order>();
        try
        {  
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                string query = "SELECT * FROM orders";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                connection.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DateTime dt = Convert.ToDateTime(reader["OrderDate"]);
                        Order order = new Order(
                            Convert.ToInt32(reader["CustomerId"]),
                            dt.ToString("yyyy-MM-dd"),
                            Convert.ToDecimal(reader["Total"])                          
                            );
                        order.OrderId = Convert.ToInt32(reader["OrderId"]);
                        OrderList.Add(order);                       
                    }
                    return OrderList;
                }
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            return OrderList;
        }
    }
}