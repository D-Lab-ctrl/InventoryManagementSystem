using System;
using MySql.Data.MySqlClient;

//create a repository, which acts as intermediary between the code and the sql server
class RepositoryUser
{
    //Define the string which contains the credentials to connect to the database in the sql server 
    private string _connectionString = "server=127.0.0.1;port=3306;username=root;password=enrico94;database=Company";

    //Define a method to register an user to a table in the database "Company"
    public void Add(User user)
    {
        try
        {
            //create an object of type MySqlConnection to enable a connection with the server
            //the using term will dispose the object when not needed to avoid memory leaks
            //The object need the credentials to connect to the server(_connectionString)
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                //define the query to perform to the database
                string query = "INSERT INTO Users(Username, Role, Password, Salt) VALUES(@Username, @Role, @Password, @Salt)";

                //define a command object to execute the query safely to avoid sql injection
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Connect to the server
                connection.Open();
                cmd.Parameters.AddWithValue("@Username", user.Username);
                cmd.Parameters.AddWithValue("@Role", user.Role);
                cmd.Parameters.AddWithValue("@Password", user.passwordHash);
                cmd.Parameters.AddWithValue("Salt", user.salt);

                cmd.ExecuteNonQuery();
            }
        }catch (Exception ex)
        {
            Console.Write(ex.Message);
        }
    }
    //Define a method that check if the username is present in the database

    //The method will return a tuple with username, role, password and salt
    //The salt is neccessary to generate a hashed password 
    public User CheckCredentials(string name)
    {
        try
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                //The query select the cell where the username is the name inserted by the user during the login
                string query = "SELECT Username, Role, Password, Salt FROM Users WHERE Username = @name";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                connection.Open();
                cmd.Parameters.AddWithValue("@name", name);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string? username = reader["Username"].ToString();
                        string? role = reader["Role"].ToString();
                        string? passwordHash = reader["Password"].ToString();
                        string? salt = reader["Salt"].ToString();
                        var user = new UserService().CreateUserByRole(username, role);
                        user.passwordHash = passwordHash;
                        user.salt = salt;
                        return user;
                    }
                    return null;
                }
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
}