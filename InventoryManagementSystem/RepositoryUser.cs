using System;
using MySql.Data.MySqlClient;

//create a repository, which acts as intermediary between the code and the sql server
class RepositoryUser
{
    //Define the string which contains the credentials to connect to the database in the sql server 
    private string _connectionString = "server=127.0.0.1;port=3306;username=root;password=las;database=Company";

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
    public List<User> GetAll()
    {
        List<User> userList = new List<User>();
        try
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                UserService service = new UserService();
                string query = "SELECT * FROM users";
                MySqlCommand cmd = new MySqlCommand(query, connection);

                connection.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int Id = Convert.ToInt32(reader["Id"]);
                        string? Username = reader["Username"].ToString();
                        string? Role = reader["Role"].ToString();

                        User user = service.CreateUserByRole(Username, Role);
                        user.Id = Id;
                        userList.Add(user);
                    }
                    return userList;
                }
            }
        }catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            return userList;
        }
    }
    public void EditUserRole(string Username, string Role)
    {
        try
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                string query = "UPDATE users SET Role = @Role WHERE Username = @Username";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                connection.Open();
                
                cmd.Parameters.AddWithValue("@Role", Role);
                cmd.Parameters.AddWithValue("@Username", Username);

                cmd.ExecuteNonQuery();
                Console.WriteLine("Operation completed.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    public void DeleteUser(string Username)
    {
        try
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                string query = "DELETE FROM users WHERE Username = @Username";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                connection.Open();

                cmd.Parameters.AddWithValue("@Username", Username);

                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    public List<string> UsernameList(string name)
    {
        List<string> usernameList = new List<string>();
        try
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                //The query select the cell where the username is the name inserted by the user during the login
                string query = "SELECT Username FROM users WHERE Username = @name";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                connection.Open();
                cmd.Parameters.AddWithValue("@name", name);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string? username = reader["Username"].ToString();
                        usernameList.Add(username);
                    }
                    return usernameList;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return usernameList;
        }
    }
}