using System;
public abstract class User
{
    //Define properties of the user object 
    public int Id { get; set; } //Id is a property as it has a getter which allows to read the data stored in Id
    public string Username { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public char? Gender { get; set; }
    public string Date { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Role { get; set; }
    private string _passwordHash;
    private string _salt;

    //Define the constructor that takes 2 arguments to initialise an object of type User
    public User(string username, string role)
    {
        Username = username;
        Role = role;
    }
    public string Salt
    {
        get { return _salt; }
        set { _salt = value; }
    }
    public string PasswordHash
    {
        get { return _passwordHash; }
        set { _passwordHash = value; }
    }
}
//create 2 classes that inherit from the user class
class Admin : User
{
    public Admin(string username, string role) : base(username, role) { }
}

class Employee : User
{
    public Employee(string username, string role) : base(username, role) { }

}

class Order
{
    public int OrderId { get; set; }
    public string CustomerId { get; set; }
    public string OrderDate { get; set; }
    public double Total { get; set; }
    public string ShippingAddress { get; set; }
    public int UserId { get; set; }

    public Order(string customerId, string orderDate, double total, string shippingAddress, int userId)
    {
        CustomerId = customerId;
        OrderDate = orderDate;
        Total = total;
        ShippingAddress = shippingAddress;
        UserId = userId;
    }
}

class Customer
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }

    public Customer(string id, string name, string surname, string email, string address)
    {
        Id = id;
        Name = name;
        Surname = surname;
        Email = email;
        Address = address;
    }
}

public class Product
{
    public string Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public int StockQty { get; set; }
    public Product(string id, string name, double price, int stockqty)
    {
        Id = id;
        Name = name;
        Price = price;
        StockQty = stockqty;
    }
}
class Report
{
    public string OrderId { get; set; }
    public string ProductId { get; set; }
    public string ProductName { get; set; }
    public int Qty { get; set; }
    public double UnitPrice { get; set; }
    public string OrderDate { get; set; }
    public string CustomerId { get; set; }
    public string CustomerName { get; set; }
    public string ShippingAddress { get; set; }
    public string UserId { get; set; }
    public string UserName { get; set; }

    public Report(string orderId, string productId, string productName, int qty, double unitPrice, string orderDate, string customerId, string customerName, string shippingAddress, string userId, string userName)
    {
        OrderId = orderId;
        ProductId = productId;
        ProductName = productName;
        Qty = qty;
        UnitPrice = unitPrice;
        OrderDate = orderDate;
        CustomerId = customerId;
        CustomerName = customerName;
        ShippingAddress = shippingAddress;
        UserId = userId;
        UserName = userName;
    }
}