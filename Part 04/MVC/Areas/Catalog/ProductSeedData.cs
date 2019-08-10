using MVC.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Areas.Catalog
{
    public class ProductSeedData
    {
        public static async Task<List<Product>> GetProducts()
        {
            var json = await File.ReadAllTextAsync("Areas/Catalog/products.json");
            var data = JsonConvert.DeserializeObject<List<ProductData>>(json);

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
    }

    public class ProductData
    {
        public int number { get; set; }
        public string name { get; set; }
        public string category { get; set; }
        public decimal price { get; set; }
    }
}