using System;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Data;

class UserService
{
    RepositoryUser repo = new RepositoryUser();
    OrderService service = new OrderService();
    ProductService product = new ProductService();
    //define a method to create an user type
    public User CreateUserByRole(string username, string role)
    {
        return role == "Admin" ? new Admin(username, role) : new Employee(username, role);
    }

    //define a method to create an user 
    public User CreateUser()
    {
        string role = repo.GetAll().Count == 0 ? "Admin" : "";
        string username;
        while (true)
        {
            username = UserUsername();
            if (UsernameExist(username) == null)
            {
                break;
            }
            Console.WriteLine("This username already exist");
        }
        string name = UserName();
        string surname = UserSurname();
        char gender = UserGender();
        string date = UserDateOfBirth();
        string email = UserEmail();
        string phone = UserPhone();
        Tuple<string, string> P_S = GeneratePassword();
        string password = P_S.Item1;
        string salt = P_S.Item2;

        User user = new Admin(username, role);
        user.Name = name;
        user.Surname = surname;
        user.Gender = gender;
        user.Date = date;
        user.Email = email;
        user.PhoneNumber = phone;
        user.Role = role;
        user.PasswordHash = password;
        user.Salt = salt;
        return user;
    }

   //define a method to register the user
   public void registerUser()
    {
        var user = CreateUser();
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
    public User LogIn()
    {
        string? username;
        while (true)
        {
            Console.WriteLine("Username: ");
            username = Console.ReadLine();
            if (UsernameExist(username) != null)
            {
                break;
            }
            Console.WriteLine("No account has this username!");
        }
        var user = repo.GetCredentials(username);
        if(user.Role == "")
        {
            Console.WriteLine("Access denied! Admin needs to approve your registration");
            return LogIn();
        }
        string inputPassword;
        while (true)
        {
            Console.WriteLine("Enter the password: ");
            inputPassword = Console.ReadLine();
            if (!string.IsNullOrEmpty(inputPassword))
            {
                string passwordHash = createPasswordHash(inputPassword, user.Salt);
                if (passwordHash == user.PasswordHash)
                {
                    Console.WriteLine("You are logged in!");
                    break;
                }
                else
                {
                    Console.WriteLine("The password is wrong!");
                    continue;
                }
            }
            else
            {
                continue;
            }
        }
        return user;
    }
    public void BasicUserOptions(User user)
    {
        while (true)
        {
            Console.WriteLine("1.\tPlace Order\n2.\tSee all orders\n3.\tSee products\n4.\tPrint order details\n5.\tExit\n");
            int choice;
            if (int.TryParse(Console.ReadLine(), out choice))
            {
                switch (choice)
                {
                    case 1:
                        service.PlaceOrder(user);
                        break;
                    case 2:
                        service.DisplayOrders();    
                        break;
                    case 3:
                        product.DisplayProducts();
                        break;
                    case 4:
                        service.PrintReport();
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
    public void AdminOptions(User user)
    {
        while (true)
        {
            Console.WriteLine("1.\tManage employee\n2.\tSee all users\n3.\tPlace order\n4.\tSee all orders\n5.\tSee products\n6.\tPrint order details\n7.\tExit\n");
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
                        service.PlaceOrder(user);
                        break;
                    case 4:
                        service.DisplayOrders();
                        break;
                    case 5:
                        product.DisplayProducts();
                        break;
                    case 6:
                        service.PrintReport();
                        break;
                    case 7:
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
        Console.WriteLine("1.\tConfirm user registration\n2.\tDelete User\n3.\tExit");
        if(int.TryParse(Console.ReadLine(), out int choice))
        {
            switch (choice)
            {
                case 1:
                    Console.WriteLine("Enter the username you want to approve the registration of: ");
                    string username = Console.ReadLine();
                    if (UsernameExist(username) != null)
                    {
                        if (repo.GetCredentials(username).Role == "")
                        {
                            Console.WriteLine("Enter the new role: ");

                            string role = Console.ReadLine();
                            if(role != "Admin" && role != "Employee") 
                            {
                                Console.WriteLine("The role must be either 'Admin' or 'Employee'");
                                break; 
                            }
                            repo.EditUserRole(username, role);
                            break;
                        }
                        else
                        {
                            Console.WriteLine("This account has been approved already!");
                            break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("No username found!");
                        break;
                    }
                        
                case 2:
                    Console.WriteLine("Enter the user's username to delete: ");
                    string name = Console.ReadLine();
                    if (UsernameExist(name) != null)
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
    public Tuple<string, string> GeneratePassword()
    {
        while (true)
        {
            Console.WriteLine("Enter a password: ");
            string inputPassword = Console.ReadLine();

            if (passwordValidation(inputPassword))
            {
                //call password generator code and return hash password and salt inside a tuple
                string salt = createSalt();
                string passwordHash = createPasswordHash(inputPassword, salt);
                Tuple <string, string> P_S = new Tuple<string, string>(passwordHash, salt);
                return P_S;
            }
            continue;
        }
    }
    public string UserUsername()
    {
        Console.WriteLine("Choose your username: ");
        string username = Console.ReadLine();
        if (Regex.IsMatch(username, @"[a-zA-Z0-9_.]+$"))
        {
            return username;
        }
        else
        {
            Console.WriteLine("The Username format is invalid");
            return UserUsername();
        }

    }
    public string UserName()
    {
        while (true)
        {
            Console.WriteLine("Enter your Name: ");
            string name = Console.ReadLine();
            if (Regex.IsMatch(name, @"[a-zA-Z]+$"))
            {
                name = char.ToUpper(name[0]) + name.Substring(1);
                return name;
            }
            else
            {
                Console.WriteLine("The name is invalid");
                continue;
            }
        }
    }
    public string UserSurname()
    {
        while (true)
        {
            Console.WriteLine("Enter your Surname: ");
            string surname = Console.ReadLine();
            if (Regex.IsMatch(surname, @"[a-zA-Z]+$"))
            {
                surname = char.ToUpper(surname[0]) + surname.Substring(1);
                return surname;
            }
            else
            {
                Console.WriteLine("The surname is invalid");
                continue;
            }
        }
    }
    public char UserGender()
    {
        while (true)
        {
            Console.WriteLine("Gender [M/F]: ");
            string input = Console.ReadLine();
            if (input.Length == 1 && Regex.IsMatch(input, @"[mMfF]$"))
            {
                
                char gender = char.ToUpper(input[0]);
                return gender;
            }
            else
            {
                Console.WriteLine("Invalid input. PLease enter 'M' or 'F'.");
                continue;
            }
        }
    }
    public string UserEmail()
    {
        while (true)
        {
            Console.WriteLine("Enter your email: ");
            string input = Console.ReadLine();
            if (Regex.IsMatch(input, @"^[a-zA-Z0-9._-]+@[a-zA-Z]+\.[a-zA-Z]{2,}$"))
            {

                string email = input.ToLower();
                return email;
            }
            else
            {
                Console.WriteLine("Invalid input. The email must be of this format e.g.: 'yourname@email.com'.");
                continue;
            }
        }
    }
    public string UserPhone()
    {
        while (true)
        {
            Console.WriteLine("Enter your phone number: ");
            string number = Console.ReadLine();
            if (Regex.IsMatch(number, @"^[+][0-9]{1,3}\s[0-9]{1,10}$"))
            {
                return number;
            }
            else
            {
                Console.WriteLine("Invalid phone number. The number must be of this format e.g.: '+123 0123456789'");
                continue;
            }
        }
    }
    public string UserDateOfBirth()
    {
        while (true)
        {
            Console.WriteLine("Enter your date of birth: ");
            string date = Console.ReadLine();
            if (isDateValid(date))
            {
                return date;
            }
            else
            {
                Console.WriteLine("Invalid input. The date must be of this format: 'yyyy-MM-dd'");
                continue;
            }
        }
    }
    public bool isDateValid(string date)
    {
        return DateOnly.TryParseExact(date, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out _);
    }
    public User UsernameExist(string username)
    {
        var user = repo.GetCredentials(username);

        return (user != null && user.Username == username) ? user : null;
    }
}