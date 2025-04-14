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
        //Define a list of acceptable roles to define as credential of the user to register
        List<string> roles = new List<string>{"Admin", "Employee"};
        //Define while loops to iterate over and over until all the registration criterias are met
        while (true)
        {   
            Console.WriteLine("Enter a Username: ");
            string? username = Console.ReadLine();
            if (!string.IsNullOrEmpty(username))
            {
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