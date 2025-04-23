using System;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryManagementSystem
{
    class Program
    {
        public static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddScoped<IUserService, UserService>()
                .AddScoped<IOrderService, OrderService>()
                .AddScoped<IProductService, ProductService>()
                .BuildServiceProvider();

            while (true)
            {   
                Console.WriteLine("1.\tLogin\n2.\tRegister\n3.\tExit");
                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    if (choice == 1)
                    {
                        var loginService = serviceProvider.GetService<IUserService>();
                        var user = loginService.LogIn();
                        if (user != null)
                        {
                            user.Options(user);
                            continue;
                        }
                    }
                    else if (choice == 2)
                    {
                        var registration = serviceProvider.GetService<IUserService>();
                        registration.registerUser();
                        continue;
                    }
                    else if (choice == 3)
                    {
                        return;
                    }
                    else
                    {
                        Console.WriteLine("You must insert a number (1, 2 or 3) corresponding to the option number.");
                    }
                }
                else
                {
                    Console.WriteLine("You must insert a number (1, 2 or 3) corresponding to the option number.");
                    continue;
                }
            }

        }
    }
}