using System;
using System.Net.Quic;
using MySql.Data.MySqlClient;
class RepositoryOrder
{
    private string _connectionString = "server=127.0.0.1;port=3306;username=root;password=las;database=Company";
    public void AddOrder(Order order)
    {
        try
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                string query = "INSERT INTO Orders(CustomerId, OrderDate, TotalAmount, Shipping_address, Handled_by) VALUES (@CustomerId, @OrderDate, @TotalAmount ,@Shipping_address, @Handled_by)";
                MySqlCommand cmd = new MySqlCommand(query, connection);

                connection.Open();

                cmd.Parameters.AddWithValue("@CustomerId", order.CustomerId);
                cmd.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                cmd.Parameters.AddWithValue("@TotalAmount", order.Total);
                cmd.Parameters.AddWithValue("@Shipping_address", order.ShippingAddress);
                cmd.Parameters.AddWithValue("@Handled_by", order.UserId);

                cmd.ExecuteNonQuery();
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    public int GetLastOrderId()
    {
        try
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                string query = "SELECT max(OrderId) FROM orders";
                MySqlCommand cmd = new MySqlCommand(query, connection);

                connection.Open();
                var Id = cmd.ExecuteScalar();

                return Convert.ToInt32(Id);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return default;
        }
    }
    public void AddOrderDetail(int orderId, Product product, int qty, double total)
    {
        try
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                string query = "INSERT INTO orderdetails(OrderId, ProductId, Qty, UnitPrice, Subtotal, Total_price) VALUES (@OrderId, @ProductId, @Qty, @UnitPrice, @Subtotal, @Total_price)";
                MySqlCommand cmd = new MySqlCommand(query, connection);

                connection.Open();

                cmd.Parameters.AddWithValue("@OrderId", orderId);
                cmd.Parameters.AddWithValue("@ProductId", product.Id);
                cmd.Parameters.AddWithValue("@Qty", qty);
                cmd.Parameters.AddWithValue("@UnitPrice", product.Price);
                cmd.Parameters.AddWithValue("@Subtotal", product.Price*qty);
                cmd.Parameters.AddWithValue("@Total_price", total);

                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    public List<Order> GetAllOrders()
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
                            reader["CustomerId"].ToString(),
                            dt.ToString("yyyy-MM-dd"),
                            Convert.ToDouble(reader["TotalAmount"]),
                            reader["Shipping_address"].ToString(),
                            Convert.ToInt32(reader["Handled_by"])
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
    public List<string> GetAllOrdersId()
    {
        List<string> OrderDetailIdList = new List<string>();
        try
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                string query = "SELECT OrderId FROM orderdetails";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                connection.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string Id = reader["OrderId"].ToString();
                        OrderDetailIdList.Add(Id);
                    }
                    return OrderDetailIdList;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return OrderDetailIdList;
        }
    }    
    public List<Report> GetOrderDetailsByOrderId(int Id)
    {
        try
        {
            List<Report> orderDetails = new List<Report>();
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                string query = "select orders.OrderId, products.Id, products.Name, orderdetails.Qty, " +
                    "orderdetails.UnitPrice, orderdetails.Subtotal, orders.OrderDate, orders.CustomerId, " +
                    "customers.Name as Customer_Name, orders.Shipping_address, orders.Handled_by as " +
                    "Processed_by, users.Name as User_Name from orderdetails join orders on orderdetails.OrderId = orders.OrderId " +
                    "join products on orderdetails.ProductId = products.Id " +
                    "join customers on orders.CustomerId = customers.Id " +
                    "join users on orders.Handled_by = users.Id " +
                    "where orders.OrderId = @Id";
                MySqlCommand cmd = new MySqlCommand(query, connection);

                connection.Open();

                cmd.Parameters.AddWithValue("@Id", Id);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DateTime dt = Convert.ToDateTime(reader["OrderDate"]);
                        orderDetails.Add(new Report(
                            reader["OrderId"].ToString(),
                            reader["Id"].ToString(),
                            reader["Name"].ToString(),
                            Convert.ToInt32(reader["Qty"]),
                            Convert.ToDouble(reader["UnitPrice"]),
                            dt.ToString("yyyy-MM-dd"),
                            reader["CustomerId"].ToString(),
                            reader["Customer_Name"].ToString(),
                            reader["Shipping_address"].ToString(),
                            reader["Processed_by"].ToString(),
                            reader["User_Name"].ToString()
                            ));
                    }
                    return orderDetails;
                }

            }
        }catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            return default;
        }
    }

}