using System.Collections.Generic;
using System.Threading.Tasks;
using API.Catalog.Models;

namespace API.Catalog.Data
{
    public interface IProductService
    {
        Task<List<Product>> GetProductsAsync();
        Task<List<Product>> SearchProductsAsync(string text);
    }
}