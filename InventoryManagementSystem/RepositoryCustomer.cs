using System;
using System.Runtime.CompilerServices;
using MySql.Data.MySqlClient;
using Mysqlx.Session;

class RepositoryCustomer
{
    private string _connectionString = "server=127.0.0.1;port=3306;username=root;password=las;database=Company";
    public void Add(Customer customer)
    {
        using(MySqlConnection connection = new MySqlConnection(_connectionString))
        {
            string query = "INSERT INTO customers(Id, Name, Surname, Email, Address) VALUES (@Id, @Name, @Surname, @Email, @Address)";
            MySqlCommand cmd = new MySqlCommand(query, connection);

            connection.Open();

            cmd.Parameters.AddWithValue("@Id", customer.Id);
            cmd.Parameters.AddWithValue("@Name", customer.Name);
            cmd.Parameters.AddWithValue("@Surname", customer.Surname);
            cmd.Parameters.AddWithValue("@Email", customer.Email);
            cmd.Parameters.AddWithValue("@Address", customer.Address);

            cmd.ExecuteNonQuery();
        }
    }
    public Customer? GetCredential(string email)
    {
        using(MySqlConnection connection = new MySqlConnection(_connectionString))
        {
            string query = "SELECT * FROM customers where Email = @email";
            MySqlCommand cmd = new MySqlCommand(query, connection);

            connection.Open();
            cmd.Parameters.AddWithValue("@email", email);

            using(MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    return new Customer(
                        reader["Id"].ToString(),
                        reader["Name"].ToString(),
                        reader["Surname"].ToString(),
                        reader["Email"].ToString(),
                        reader["Address"].ToString()
                        );
                }
                return default;
            }
        }
    }

    public List<string> GetAllCustomersId()
    {
        List<string> customerIdlist = new List<string>();
        using(MySqlConnection connection = new MySqlConnection(_connectionString))
        {
            string query = "SELECT Id from customers";
            MySqlCommand cmd = new MySqlCommand(query, connection);

            connection.Open();

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    customerIdlist.Add(reader["Id"].ToString());
                }
                return customerIdlist;
            }

        }
    }
    public List<string> GetAllCustomersEmail()
    {
        List<string> customerEmailist = new List<string>();
        using (MySqlConnection connection = new MySqlConnection(_connectionString))
        {
            string query = "SELECT Email from customers";
            MySqlCommand cmd = new MySqlCommand(query, connection);

            connection.Open();

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    customerEmailist.Add(reader["Email"].ToString());
                }
                return customerEmailist;
            }

        }
    }
}
