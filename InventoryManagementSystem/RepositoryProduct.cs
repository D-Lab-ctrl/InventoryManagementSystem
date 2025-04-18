using System;
using MySql.Data.MySqlClient;

class RepositoryProduct
{
    private string _connectionString = "server=127.0.0.1;port=3306;username=root;password=las;database=Company";

    public List<Product> GetProducts()
    {
        List<Product> productList = new List<Product>();
        using(MySqlConnection connection = new MySqlConnection(_connectionString))
        {
            string query = "SELECT * FROM products";
            MySqlCommand cmd = new MySqlCommand(query, connection);

            connection.Open();
            using(MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Product product = new Product(
                        reader["Id"].ToString(),
                        reader["Name"].ToString(),
                        Convert.ToDouble(reader["Price"]),
                        Convert.ToInt32(reader["Stock_Qty"])
                        );
                    productList.Add(product);
                }
                return productList;
            }
        }
    }
    public Product GetProductsInfo(string productId)
    {
        using (MySqlConnection connection = new MySqlConnection(_connectionString))
        {
            string query = "SELECT * FROM products where Id = @Id";
            MySqlCommand cmd = new MySqlCommand(query, connection);

            cmd.Parameters.AddWithValue("@Id", productId);

            connection.Open();
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Product product = new Product(
                        reader["Id"].ToString(),
                        reader["Name"].ToString(),
                        Convert.ToDouble(reader["Price"]),
                        Convert.ToInt32(reader["Stock_Qty"])
                        );
                    return product;
                }
                return default;
            }
        }
    }

    public List<string> ProductIdList()
    {
        List<string> productIdList = new List<string>();
        try
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                //The query select the cell where the username is the name inserted by the user during the login
                string query = "SELECT Id FROM products";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                connection.Open();

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string? Id = reader["Id"].ToString();
                        productIdList.Add(Id);
                    }
                    return productIdList;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return productIdList;
        }
    }

    public void UpdateStock(Product product, int qty)
    {
        using (MySqlConnection connection = new MySqlConnection(_connectionString))
        {
            string query = "UPDATE products SET Stock_Qty = @newQty where Id = @Id";
            MySqlCommand cmd = new MySqlCommand(query, connection);

            int newQty = product.StockQty - qty;

            connection.Open();

            cmd.Parameters.AddWithValue("@newQty", newQty);
            cmd.Parameters.AddWithValue("Id", product.Id);

            cmd.ExecuteNonQuery();
        }
    }
}
