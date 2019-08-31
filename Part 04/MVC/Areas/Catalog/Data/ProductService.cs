using MVC.Areas.Catalog.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MVC.Areas.Catalog.Data
{
    public class ProductService : IProductService
    {
        private const string fileName = "Data/products.json";

        public async Task<List<Product>> GetProductsAsync()
        {
            List<ProductData> data = await GetProductDataFromFile();

            List<Product> products = GetProducts(data);

            return products;
        }

        public List<Product> GetProducts()
        {
            return GetProductsAsync().Result;
        }

        private static List<Product> GetProducts(List<ProductData> data)
        {
            var dict = new Dictionary<string, Category>();

            var categories =
                data
                .Select(i => i.category)
                .Distinct();

            foreach (var name in categories)
            {
                var category = new Category(dict.Count + 1, name);
                dict.Add(name, category);
            }

            var products = new List<Product>();

            foreach (var item in data)
            {
                Product product = new Product(
                    products.Count + 1,
                    item.number.ToString("000"),
                    item.name,
                    item.price,
                    dict[item.category]);
                products.Add(product);
            }

            return products;
        }

        private static async Task<List<ProductData>> GetProductDataFromFile()
        {
            string json = await File.ReadAllTextAsync("areas/catalog/data/products.json");
            return JsonConvert.DeserializeObject<List<ProductData>>(json);
        }
    }

    public class ProductData
    {
        public int number { get; set; }
        public string name { get; set; }
        public string category { get; set; }
        public decimal price { get; set; }
    }
}