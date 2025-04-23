public interface IProductService
{
    public void DisplayProducts();
    public List<Tuple<Product, int>> InsertProducts();
    public string FindProduct();
    public int GetQty(string Id);
}