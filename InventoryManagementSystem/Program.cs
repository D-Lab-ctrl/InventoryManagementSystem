using System;

namespace InventoryManagementSystem
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("1.\tLogin\n2.\tRegister\n");
            int choice = Convert.ToInt32(Console.ReadLine());
            if(choice == 1)
            {
                UserService login = new UserService();
                var user = login.LogIn();
                if(user != null)
                {
                    user.Options();
                }
                
            }            
            if(choice == 2)
            {
                UserService registration = new UserService();
                registration.registerUser();

            }
        }
    }
}
