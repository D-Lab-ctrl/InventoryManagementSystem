using System;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

class UserService
{   
   //define a method to create an user type
   public User CreateUserByRole(string username, string role)
    {
        return role == "Admin" ? new Admin(username, role) : new Employee(username, role);
    }

    //define a method to create an user 
    public User CreateUser()
    {
        //Define a list of acceptable roles to define as credential of the user to register
        List<string> roles = ["Admin", "Employee"];
        Console.WriteLine("Enter a Username: ");
        string? username = Console.ReadLine();
        if (!string.IsNullOrEmpty(username))
        {
            Console.WriteLine("Enter the role: ");
            string? role = Console.ReadLine();
            if (roles.Contains(role))
            {
                Console.WriteLine("Enter a password: ");
                string? inputPassword = Console.ReadLine();

                //call password generator code and return hash password and salt
                //create user object then generate password and salt and assign them to the object fields.

                var user = CreateUserByRole(username, role);
                user.salt = createSalt();
                user.passwordHash = createPasswordHash(inputPassword, user.salt);
                return user;
            }
            else
            {
                Console.WriteLine("The role inserted is not valid.");
                return null;
            }
        }
        else
        {
            Console.WriteLine("Username cannot be empty or null.");
            return null;
        }
    }

   //define a method to register the user
   public void registerUser()
    {
        var user = CreateUser();
        RepositoryUser repo = new RepositoryUser();
        repo.Add(user);
    }

    //Define a method where the user give in input username and password and then the result fetched by
    //the repository is compared with the input to see if there is a match and therefore if the login is valid
    public User LogIn()
    {
        Console.WriteLine("Enter your Username: ");
        string? input = Console.ReadLine();
        RepositoryUser repo = new RepositoryUser();
        Tuple<string, string, string> value = repo.CheckUsername(input);
        if (value.Item1 == input)
        {
            Console.WriteLine("Enter the password: ");
            string? password = Console.ReadLine();
            string? passwordHash = createPasswordHash(password, value.Item2);
            string? secondValue = repo.CheckPassword(value.Item1, passwordHash);
            if(secondValue == passwordHash)
            {
                Console.WriteLine("You are logged in!\n");
                var user = CreateUserByRole(value.Item1, value.Item3);
                return user;                
            }
            else
            {
                Console.WriteLine("Wrong Password.");
                return null;
            }
        }
        else
        {
            Console.WriteLine("Username not found!");
            return null;
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

}