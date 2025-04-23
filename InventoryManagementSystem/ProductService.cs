using System;
using System.Collections.Generic;
using Org.BouncyCastle.Asn1;

class ProductService
{
    RepositoryProduct repo = new RepositoryProduct();
    public void DisplayProducts()
    {
        List<Product> products = repo.GetProducts();

        Console.WriteLine(new string('-', 50));
        Console.WriteLine($"{"ProductId", -9} | {"Name", -12} | {"Price [£]", 10} | {"Stock Qty", 10}");
        Console.WriteLine("----------+--------------+------------+-----------");
        foreach (Product product in products)
        {

            Console.WriteLine($"{product.Id,-9} | {product.Name,-12} | {product.Price,10} | {product.StockQty,10}");
            Console.WriteLine(new string('-', 50));
        }

    }
    public List<Tuple<Product, int>> InsertProducts()
    {
        
        List<Tuple<Product, int>> productList = new List<Tuple<Product, int>>();
        while (true)
        {
            string productId = FindProduct();
            while (true)
            {
                if (productId == "exit" && productList.Count>0) 
                { 
                    return productList; 
                }
                else if(productId == "exit" && productList.Count == 0)
                {
                    Console.WriteLine("You must insert products in the order.");
                    break;
                }
                else
                {
                    int productQty = GetQty(productId);
                    Product product = repo.GetProductsInfo(productId);
                    Tuple<Product, int> orderedProduct = new Tuple<Product, int>(product, productQty);
                    productList.Add(orderedProduct);
                    break;
                }
            }
        }
    }
    public string FindProduct()
    {
        while (true)
        {
            Console.WriteLine("Enter the product code: ");
            string code = Console.ReadLine();
            if (code == "exit")
            {
                return "exit";
            }
            if (repo.GetProductsInfo(code) == null)
            {
                Console.WriteLine($"No product available with the code: {code}.");
                continue;
            }
            else
            { 
                return code; 
            }
        }
    }
    public int GetQty(string Id)
    {
        while (true)
        {
            Console.WriteLine("Enter the product qty: ");
            if (int.TryParse(Console.ReadLine(), out int qty))
            {
                int actualQty = repo.GetProductsInfo(Id).StockQty;
                if (actualQty >= qty)
                {
                    return qty;
                }
                else
                {
                    Console.WriteLine($"Not enough stock. Current qty is: {actualQty}");
                }
            }
            else
            {
                Console.WriteLine("Invalid Input");
            }
        }
    }
}