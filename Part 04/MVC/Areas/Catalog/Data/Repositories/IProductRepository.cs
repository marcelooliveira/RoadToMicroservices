using MVC.Areas.Catalog.Models;
using MVC.Areas.Catalog.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MVC.Areas.Catalog.Data.Repositories
{
    public interface IProductRepository
    {
        void Initialize();
        Task<IList<Product>> GetProductsAsync();
        Task<Product> GetProductAsync(string code);
        Task<SearchProductsViewModel> GetProductsAsync(string searchText);
    }
}
