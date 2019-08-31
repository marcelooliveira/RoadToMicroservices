using MVC.Areas.Catalog.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MVC.Areas.Catalog.Data
{
    public interface IProductService
    {
        List<Product> GetProducts();
        Task<List<Product>> GetProductsAsync();
    }
}