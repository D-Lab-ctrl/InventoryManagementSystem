using System;
abstract class User
{
    //Define properties of the user object 
    public int Id { get; set; } //Id is a property as it has a getter which allows to read the data stored in Id
    public string? Username { get; set; }
    public string? Role { get; set; }
    private string? _passwordHash;
    private string? _salt;

    //Define the constructor that takes 2 arguments to initialise an object of type User
    public User(string username, string role)
    {
        Username = username;
        Role = role;
    }
    public string salt
    {
        get { return _salt; }
        set { _salt = value; }
    }
    public string passwordHash
    {
        get { return _passwordHash; }
        set { _passwordHash = value; }
    }
    //defining the method as virtual allows to override it
    public virtual void Options()
    {
        UserService service = new UserService();
        service.BasicUserOptions();
    }
}
//create 2 classes that inherit from the user class
class Admin : User
{
    public Admin(string username, string role) : base(username, role) { }

    public override void Options()
    {
        UserService service = new UserService();
        service.AdminOptions();
    }
}

class Employee : User
{
    public Employee(string username, string role) : base(username, role) { }

}

class Order
{
    public int OrderId { get; set; }
    public int CustomerId { get; set; }
    public string OrderDate { get; set; }
    public decimal Total { get; set; }

    public Order(int customerId, string orderDate, decimal total)
    {
        CustomerId = customerId;
        OrderDate = orderDate;
        Total = total;
    }
}