using System;
class OrderService
{
    public void PlaceOrder()
    {
        while (true)
        {
            Console.WriteLine("Enter the CustomerId: ");
            if (int.TryParse(Console.ReadLine(), out int customerId))
            {
                Console.WriteLine("Enter the date: ");
                string dateOrder = Console.ReadLine();
                if (ValidateDate(dateOrder))
                {
                    Console.WriteLine("Enter the total[£].");
                    if (decimal.TryParse(Console.ReadLine(), out decimal total))
                    {
                        Order order = new Order(customerId, dateOrder, total);
                        RepositoryOrder repo = new RepositoryOrder();
                        repo.Add(order);
                        return;
                    }
                    else
                    {
                        Console.WriteLine("The amount must be a decimal type.");
                        continue;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid date format, must be  'yyyy-MM-dd'");
                    continue;
                }

            }
            else
            {
                Console.WriteLine("The customerId must be a number.");
                continue;
            }
        }

    }
    public bool ValidateDate(string date)
    {
        return DateOnly.TryParseExact(date, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out _);
    }

    public void DisplayOrders()
    {
        RepositoryOrder repo = new RepositoryOrder();
        List<Order> orderlist = repo.GetAll();
        if (orderlist.Count == 0)
        {
            Console.WriteLine("----------------------------------\n");
            Console.WriteLine("No order found");
            Console.WriteLine("----------------------------------\n");
        }
        else
        {
            foreach (Order order in orderlist)
            {
                Console.WriteLine("----------------------------------");
                Console.WriteLine($"OrderId: {order.OrderId}\nCustomerId: {order.CustomerId}\nOrderDate: {order.OrderDate}\nTotal: {order.Total}");
            }
            Console.WriteLine("----------------------------------\n");
        }
    }
}