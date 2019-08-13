using API.Catalog.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace API.Catalog.Data
{
    public class ProductService : IProductService
    {
        private const string fileName = "Data/data.json";

        public async Task<List<Product>> GetProductsAsync()
        {
            List<ProductData> data = await GetProductDataFromFile();

            List<Product> products = GetProducts(data);

            return products;
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
            string json;
            string location = Assembly.GetExecutingAssembly().Location;
            Debug.WriteLine(location);
            string directory = Path.GetDirectoryName(location);
            using (var sourceReader =
                File.OpenText(Path.Combine(directory, fileName)))
            {
                json = await sourceReader.ReadToEndAsync();
            }

            var data = JsonConvert.DeserializeObject<List<ProductData>>(json);
            return data;
        }

        public async Task<List<Product>> SearchProductsAsync(string searchText)
        {
            searchText = searchText ?? "";
            searchText = searchText.Trim();

            List<ProductData> data = await GetProductDataFromFile();
            var filtered = data.Where(p => p.name.ToLower().Contains(searchText.ToLower()) ||
                    p.category.ToLower().Contains(searchText.ToLower()))
                .ToList();
            List<Product> products = GetProducts(filtered);

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