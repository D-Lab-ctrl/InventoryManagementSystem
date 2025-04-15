using System;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

class UserService
{   
   //define a method to create an user type
   public User CreateUserByRole(string username, string role)
    {
        return role == "Admin" ? new Admin(username, role) : new Employee(username, role);
    }

    //define a method to create an user 
    public User? CreateUser()
    {
        RepositoryUser repo = new RepositoryUser();
        //Define a list of acceptable roles to define as credential of the user to register
        List<string> roles = new List<string>{"Admin", "Employee"};
        //Define while loops to iterate over and over until all the registration criterias are met
        while (true)
        {   
            Console.WriteLine("Enter a Username: ");
            string? username = Console.ReadLine();
            if (!string.IsNullOrEmpty(username))
            {
                if(!repo.UsernameList(username).Contains(username)) {
                    while (true)
                    {
                        Console.WriteLine("Enter the role: ");
                        string? role = Console.ReadLine();
                        if (!string.IsNullOrEmpty(role))
                        {
                            if (roles.Contains(role))
                            {
                                while (true)
                                {
                                    Console.WriteLine("Enter a password: ");
                                    string? inputPassword = Console.ReadLine();

                                    if (passwordValidation(inputPassword))
                                    {
                                        //call password generator code and return hash password and salt
                                        //create user object then generate password and salt and assign them to the object fields.

                                        var user = CreateUserByRole(username, role);
                                        user.salt = createSalt();
                                        user.passwordHash = createPasswordHash(inputPassword, user.salt);
                                        return user;
                                    }
                                    continue;
                                }
                            }
                            else
                            {
                                Console.WriteLine("The role inserted is not valid.\nIt must be either 'Admin' or 'Employee'");
                                continue;
                            }
                        }
                        else
                        {
                            Console.WriteLine("The role cannot be empty or null.");
                            continue;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("The Username already exist.");
                }
            }
            else
            {
                Console.WriteLine("Username cannot be empty or null.");
                continue;
            }
        }
    }

   //define a method to register the user
   public void registerUser()
    {
        var user = CreateUser();
        RepositoryUser repo = new RepositoryUser();
        try
        {
            repo.Add(user);
            Console.WriteLine("Registration completed!");
        }
        catch(Exception ex)
        {
            Console.WriteLine("Registration failed!");
            Console.WriteLine(ex.Message);
        }
        
    }

    //Define a method where the user give in input username and password and then the result fetched by
    //the repository is compared with the input to see if there is a match and therefore if the login is valid
    //add while loop to keep trying to log in
    public User? LogIn()
    {
        while (true)
        {
            Console.WriteLine("Enter your Username: ");
            string? input = Console.ReadLine();
            if (!string.IsNullOrEmpty(input))
            {
                RepositoryUser repo = new RepositoryUser();
                var value = repo.CheckCredentials(input);
                if (value == null)
                {
                    Console.WriteLine("Username not found");
                    continue;
                }
                else
                {
                    Console.WriteLine("Enter the password: ");
                    string? password = Console.ReadLine();
                    if (!string.IsNullOrEmpty(password))
                    {
                        string? passwordHash = createPasswordHash(password, value.salt);
                        if (value.passwordHash == passwordHash)
                        {
                            Console.WriteLine("You are logged in!\n");
                            var user = CreateUserByRole(value.Username, value.Role);
                            return user;
                        }
                        else
                        {
                            Console.WriteLine("The password you entered is wrong.");
                            continue;
                        }
                    }
                    else
                    {
                        Console.WriteLine("The password cannot be empty or null");
                        continue;
                    }
                }
            }
            else
            {
                Console.WriteLine("Username cannot be empty or null.");
                continue;
            }
        }
    }
    public void BasicUserOptions()
    {
        OrderService service = new OrderService();
        while (true)
        {
            Console.WriteLine("1.\tPlace Order\n2.\tSee all orders\n3.\tExit\n");
            int choice;
            if (int.TryParse(Console.ReadLine(), out choice))
            {
                switch (choice)
                {
                    case 1:
                        service.PlaceOrder();
                        break;
                    case 2:
                        service.DisplayOrders();    
                        break;
                    case 3:
                        return;
                    default:
                        Console.WriteLine("You must enter a number corresponding to the options available (1, 2, 3)");
                        break;
                }
            }
            else
            {
                Console.WriteLine("You must enter a number corresponding to the options available (1, 2, 3)");
            }
        }
    }
    public void AdminOptions()
    {
        OrderService service = new OrderService();
        while (true)
        {
            Console.WriteLine("1.\tManage employee\n2.\tSee all users\n3.\tPlace order\n4.\tSee all orders\n5.\tExit\n");
            int choice;
            if (int.TryParse(Console.ReadLine(), out choice))
            {
                switch (choice)
                {
                    case 1:
                        ManageUser();
                        break;
                    case 2:
                        DisplayUsers();
                        break;
                    case 3:
                        service.PlaceOrder();
                        break;
                    case 4:
                        service.DisplayOrders();
                        break;
                    case 5:
                        return;
                    default:
                        Console.WriteLine("You must enter a number corresponding to the options available (1, 2, 3)");
                        break;
                }
            }
            else
            {
                Console.WriteLine("You must enter a number corresponding to the options available (1, 2, 3)");
            }
        }
    }
    public void ManageUser()
    {
        RepositoryUser repo = new RepositoryUser();
        Console.WriteLine("1.\tEdit User\n2.\tDelete User\n3.\tExit");
        if(int.TryParse(Console.ReadLine(), out int choice))
        {
            switch (choice)
            {
                case 1:
                    Console.WriteLine("Enter the user's username you want to edit the role of: ");
                    string username = Console.ReadLine();
                    if (repo.UsernameList(username).Contains(username))
                    {
                        Console.WriteLine("Enter the new role: ");
                        string role = Console.ReadLine();
                        User userToEdit = repo.CheckCredentials(username);
                        if (userToEdit.Role == "Admin")
                        {
                            Console.WriteLine("Cannot change admin credentials!");
                        }
                        else
                        {
                            repo.EditUserRole(username, role);
                        }
                        break;
                    }
                    else
                    {
                        Console.WriteLine("No username found!");
                        break;
                    }
                        
                case 2:
                    Console.WriteLine("Enter the user's username to delete: ");
                    string? name = Console.ReadLine();
                    if (repo.UsernameList(name).Contains(name))
                    {
                        repo.DeleteUser(name);
                    }
                    else
                    {
                        Console.WriteLine("No username found!");
                        break;
                    }
                    break;
            }
        }
        else
        {
            Console.WriteLine("You must enter a number corresponding to the options available (1, 2, 3, 4)");
        }
    }
    public void DisplayUsers()
    {
        RepositoryUser repo = new RepositoryUser();
        List<User> userList = new List<User>(repo.GetAll());
        if (userList.Count > 0)
        {
            foreach (User user in userList)
            {
                Console.WriteLine("----------------------------------");
                Console.WriteLine($"User ID: {user.Id}\nUsername: {user.Username}\nRole: {user.Role}");
                Console.WriteLine("----------------------------------");
            }
        }
        else
        {
            Console.WriteLine("----------------------------------");
            Console.WriteLine("No user found.");
            Console.WriteLine("----------------------------------");
        }
    }
    public string createSalt()
    {
        byte[] salt = new byte[16];
        var rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);
        return Convert.ToBase64String(salt);
    }
    public string createPasswordHash(string password, string salt)
    {
        return Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: Convert.FromBase64String(salt),
            prf: KeyDerivationPrf.HMACSHA512,
            iterationCount: 100000,
            numBytesRequested: 256/8
            ));
    }

    public bool passwordValidation(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            Console.WriteLine("The password cannot be empty or null.");
            return false;
        }
        else
        {
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_])[^\s]+$";
            
            if(Regex.IsMatch(input, pattern) && input.Length >= 8)
            {
                return true;
            }
            Console.WriteLine("The password must be longer than 8 characters and contain at least:\n1 Capitol letter\n1 Number\n1 Special character\nNo spaces.");
            return false;
        }
    }
}