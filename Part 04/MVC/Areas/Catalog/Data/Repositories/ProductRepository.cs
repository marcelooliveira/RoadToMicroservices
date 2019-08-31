using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MVC.Areas.Catalog.Models;
using MVC.Areas.Catalog.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Areas.Catalog.Data.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        static List<Product> products;
        public ProductRepository(IConfiguration configuration,
            CatalogDbContext context) : base(configuration, context)
        {
        }

        public async Task<IList<Product>> GetProductsAsync()
        {
            return await dbSet
                .Include(prod => prod.Category)
                .ToListAsync();
        }

        public async Task<Product> GetProductAsync(string code)
        {
            return await dbSet
                .Where(p => p.Code == code)
                .Include(prod => prod.Category)
                .SingleOrDefaultAsync();
        }

        public async Task<SearchProductsViewModel> GetProductsAsync(string searchText)
        {
            if (products == null)
            {
                products =
                    await dbSet
                        .Include(prod => prod.Category)
                        .ToListAsync();
            }

            var result = products;

            if (!string.IsNullOrEmpty(searchText))
            {
                searchText = searchText.ToLower();
                result =
                    products
                        .Where(q =>
                        q.Name.ToLower().Contains(searchText)
                        || q.Category.Name.ToLower().Contains(searchText))
                        .ToList();
            }

            return new SearchProductsViewModel(result, searchText);
        }
    }
}
