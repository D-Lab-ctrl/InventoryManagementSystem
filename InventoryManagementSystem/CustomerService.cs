using System;
using System.Text.RegularExpressions;

class CustomerService
{
    RepositoryCustomer repo = new RepositoryCustomer();
    public void RegisterCustomer()
    {
        try
        {
            Customer customer = new Customer(CustomerId(), CustomerName(), CustomerSurame(), CustomerEmail(), CustomerAddress());
            repo.Add(customer);
        }catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    public Customer? GetCustomerCredentials(string email)
    {
        return repo.GetCredential(email);
    }
    public string CustomerId()
    {
        List<string> listId = repo.GetAllCustomersId();
        if(listId.Count == 0)
        {
            return "CUST1001";
        }

        string lastID = listId[listId.Count - 1];
        int sequence = Convert.ToInt32(lastID.Substring(4));
        int nextnum = sequence + 1;
        string nextId = "CUST" + nextnum.ToString();
        return nextId;
    }
    public string CustomerName()
    {
        while (true)
        {
            Console.WriteLine("Enter customer name: ");
            string name = Console.ReadLine();
            if (!string.IsNullOrEmpty(name))
            {
                if (Regex.IsMatch(name, @"^[a-zA-Z]+$"))
                {
                    return name;
                }
                else
                {
                    Console.WriteLine("Invalid name. It must contain only alphabetic characters.");
                    continue;
                }
            }
            else
            {
                continue;
            }
        }
    }
    public string CustomerSurame()
    {
        while (true)
        {
            Console.WriteLine("Enter customer surname: ");
            string surname = Console.ReadLine();
            if (string.IsNullOrEmpty(surname))
            {
                return CustomerName();
            }
            if (Regex.IsMatch(surname, @"^[a-zA-Z]+$"))
            {
                return surname;
            }
            else
            {
                Console.WriteLine("Invalid surname. It must contain only alphabetic characters.");
                continue;
            }
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
                return CustomerEmail();
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
                return CustomerName();
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