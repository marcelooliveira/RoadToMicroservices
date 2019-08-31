using Microsoft.EntityFrameworkCore;
using MVC.Areas.Catalog.Models;
using System.Linq;

namespace MVC.Areas.Catalog.Data
{
    //PM> Add-Migration Catalog -Context CatalogDbContext -OutputDir "Areas/Catalog/Data/Migrations"
    public class CatalogDbContext : DbContext
    {
        private const string fileName = "Data/products.json";
        private readonly IProductService productService;

        public CatalogDbContext(DbContextOptions options, IProductService productService) : base(options)
        {
            this.productService = productService;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var products = productService.GetProducts();
            var categories =
                products.Select(p => p.Category).Distinct();

            builder.Entity<Category>(b =>
            {
                b.HasKey(t => t.Id);
                b.HasData(categories);
            });

            builder.Entity<Product>(b =>
            {
                b.HasKey(t => t.Id);
                b.HasData(
                products.Select(p =>
                        new
                        {
                            p.Id,
                            p.Code,
                            p.Name,
                            p.Price,
                            CategoryId = p.Category.Id
                        }
                    ));
            });
            builder.Entity<Product>();
        }
    }
}
