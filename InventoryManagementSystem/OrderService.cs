using System;
class OrderService
{
    RepositoryOrder repoOrd = new RepositoryOrder();
    RepositoryProduct repoProd = new RepositoryProduct();
    CustomerService serviceCust = new CustomerService();
    ProductService serviceProd = new ProductService();
    public void PlaceOrder(User user)
    {
        if (repoProd.GetProducts().Count == 0)
        {
            Console.WriteLine("The are not products in the catalogue!");
            return;
        }

        List<Tuple<Product, int>> productList = serviceProd.InsertProducts();

        double Total = TotalAmount(productList);
        while (Total == 0)
        {
            Console.WriteLine("You must insert products to the order.");
            productList = serviceProd.InsertProducts();
            Total = TotalAmount(productList);
        }

        //Insert Customer email
        string? email = serviceCust.CustomerEmail();

        if (!serviceCust.EmailExist(email))
        {
            Console.WriteLine("The email does not match any customer.\n" +
                "The customer must be registered before processing the order.....");
            serviceCust.RegisterCustomer();
            Console.WriteLine("----------------------");
            Console.WriteLine("Registration complete!");
            Console.WriteLine("----------------------");
        }

        //Retrieve Customer data from CustomerRepository
        var customer = serviceCust.GetCustomerCredentials(email);

        Order order = new Order(customer.Id, DateOrder(), Total, ShippingAddress(), user.Id);
        repoOrd.AddOrder(order);
        Console.WriteLine("----------------");
        Console.WriteLine("Order sumbitted!");
        Console.WriteLine("----------------");

        //insert orderdetails to order details table;

        foreach (Tuple<Product, int> products in productList)
        {
            repoOrd.AddOrderDetail(repoOrd.GetLastOrderId(), products.Item1, products.Item2, Total);
            repoProd.UpdateStock(products.Item1, products.Item2);
        }

    }
    public string DateOrder()
    {
        while (true)
        {
            Console.WriteLine("Enter the date: ");
            string dateOrder = Console.ReadLine();
            if (ValidateDate(dateOrder))
            {
                return dateOrder;
            }
            else
            {
                Console.WriteLine("Invalid input. The date must be of the format: yyyy-MM-dd.");
                continue;
            }
        }
    }
    public double TotalAmount(List<Tuple<Product, int>> productList)
    {
        double Total = 0;
        if (productList.Count == 0) { return Total; }
        foreach (Tuple<Product, int> orderedProduct in productList)
        {
            Total += orderedProduct.Item1.Price * orderedProduct.Item2;
        }
        return Total;
    }
    public string ShippingAddress()
    {
        while (true)
        {
            Console.WriteLine("Enter the shipping address: ");
            string address = Console.ReadLine();
            if (!string.IsNullOrEmpty(address))
            {
                return address;
            }
            Console.WriteLine("Address cannot be empty. Please try again.");
        }
    }
    public bool ValidateDate(string date)
    {
        return DateOnly.TryParseExact(date, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out _);
    }
    public void DisplayOrders()
    {
        List<Order> orderlist = repoOrd.GetAllOrders();
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
                Console.WriteLine("-------------------------------------");
                Console.WriteLine($"OrderId: {order.OrderId,28}\nCustomerId: {order.CustomerId,25}\nOrderDate: {order.OrderDate,26}\nTotal: {"£ " + order.Total,30}\nShipping address: {order.ShippingAddress}\nHandled by: {order.UserId,25}");
            }
            Console.WriteLine("-------------------------------------\n");
        }
    }
    public void PrintReport()
    {
        while (true)
        {
            if (repoOrd.GetAllOrdersId().Count == 0)
            {
                Console.WriteLine("No order was processed.");
                return;
            }
            Console.WriteLine("Insert Order Id: ");
            if (int.TryParse(Console.ReadLine(), out int orderId))
            {
                if (repoOrd.GetAllOrdersId().Contains(orderId.ToString()))
                {
                    List<Report> report = repoOrd.GetOrderDetailsByOrderId(orderId);
                    Console.WriteLine(new string('-', 50));
                    Console.WriteLine("ORDER DETAILS");
                    Console.WriteLine(new string('-', 50));
                    Console.WriteLine($"Order ID: {report[0].OrderId}");
                    string productTitle = $"Products:       {report[0].Qty} x {report[0].ProductName}({report[0].ProductId})";
                    if (report.Count > 1)
                    {
                        for (int i = 1; i < report.Count; i++)
                        {
                            productTitle += $"\n                  {report[i].Qty} x {report[i].ProductName}({report[i].ProductId})";
                        }
                    }
                    Console.WriteLine(productTitle);
                    Console.WriteLine($"Customer: {report[0].CustomerName}({report[0].CustomerId})");
                    Console.WriteLine($"Shipped to: {report[0].ShippingAddress}");
                    Console.WriteLine($"Processed by: {report[0].UserName}({report[0].UserId})");
                    Console.WriteLine(new string('-', 50));
                    break;
                }
                else
                {
                    Console.WriteLine($"There is no order Id = {orderId}");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. The order ID must be a number.");
            }
        }
    }
}
