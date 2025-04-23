using System;
using System.Text.RegularExpressions;

class CustomerService
{
    RepositoryCustomer repo = new RepositoryCustomer();
    public void RegisterCustomer()
    {
        try
        {
            Customer customer = new Customer(repo.GetCustomerId(), GetName("Enter customer's name: "), GetName("Enter customer's surname: "), CustomerEmail(), CustomerAddress());
            repo.Add(customer);
        }catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    public Customer GetCustomerCredentials(string email)
    {
        return repo.GetCredential(email);
    }
    public string GetName(string prompt)
    {
        while (true)
        {
            Console.WriteLine(prompt);
            string input = Console.ReadLine();
            if (!string.IsNullOrEmpty(input) && Regex.IsMatch(input, @"^[a-zA-Z]+$"))
            {
                return input;
            }
            Console.WriteLine("Invalid name. It must contain only alphabetic characters.");
        }
    }
    public string CustomerEmail()
    {
        while (true)
        {
            Console.WriteLine("Enter customer email: ");
            string email = Console.ReadLine();
            if (string.IsNullOrEmpty(email))
            {
                continue;
            }
            if (Regex.IsMatch(email, @"^[a-zA-Z0-9._-]+@[a-z]+\.[a-z]{2,}$"))
            {
                return email;
            }
            else
            {
                Console.WriteLine("Invalid email format.");
                continue;
            }
        }
    }
    public string CustomerAddress()
    {
        while (true)
        {
            Console.WriteLine("Enter customer address: ");
            string address = Console.ReadLine();
            if (string.IsNullOrEmpty(address))
            {
                continue;
            }
            else
            {
                return address;
            }
        }
    }
    public bool EmailExist(string email)
    {
        if (repo.GetAllCustomersEmail().Contains(email))
        {
            return true;
        }
        return false;
    }
}