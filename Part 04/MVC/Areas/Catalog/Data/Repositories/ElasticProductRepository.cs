using Microsoft.Extensions.Configuration;
using MVC.Areas.Catalog.Models;
using MVC.Areas.Catalog.Models.ViewModels;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Areas.Catalog.Data.Repositories
{
    public class ElasticProductRepository : IProductRepository
    {
        private readonly IConfiguration configuration;
        private readonly IProductService productService;
        private readonly ConnectionSettings settings;
        private readonly ElasticClient client;

        public ElasticProductRepository(IConfiguration configuration, IProductService productService)
        {
            this.configuration = configuration;
            this.productService = productService;
            var node = new Uri(configuration.GetConnectionString("ElasticSearchNode"));
            settings = new ConnectionSettings(node);
            client = new ElasticClient(settings);
        }

        public void Initialize()
        {
            var products = productService.GetProducts();
            client.IndexMany(products, "product-index");
        }

        public async Task<Product> GetProductAsync(string code)
        {
            // returns an IGetResponse mapped 1-to-1 with the Elasticsearch JSON response
            var response = 
                await client
                        .GetAsync<Product>(code,
                            idx => idx.Index("product-index"));
            // the original document
            return response.Source;
        }

        public async Task<IList<Product>> GetProductsAsync()
        {
            var response = 
                await client
                .SearchAsync<Product>(s => s
                    .Index("product-index")
                    .Size(1000));

            return response.Documents.ToList();
        }

        public async Task<SearchProductsViewModel> GetProductsAsync(string searchText)
        {
            var response =
                await client
                .SearchAsync<Product>(s => s
                .Index("product-index")
                .Query(q => 
                       q.Match(mq => mq.Field(f => f.Name).Query(searchText))
                    || q.Match(mq => mq.Field(f => f.Category.Name).Query(searchText))
                )
                .Size(1000));

            var products = response.Documents.ToList();

            var searchProducts = new SearchProductsViewModel(products, searchText);

            return searchProducts;
        }
    }
}
