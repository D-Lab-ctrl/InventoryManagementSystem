public interface IOrderService
{
    public void PlaceOrder(User user);
    public double TotalAmount(List<Tuple<Product, int>> productList);
    public string ShippingAddress();
    public void DisplayOrders();
    public void PrintReport();
}
