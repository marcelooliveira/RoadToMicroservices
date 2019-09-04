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
        private const string ProductIndexName = "product-index";
        private readonly IConfiguration configuration;
        private readonly IProductService productService;
        private readonly ConnectionSettings settings;
        private readonly ElasticClient client;

        public ElasticProductRepository(IConfiguration configuration, IProductService productService)
        {
            this.configuration = configuration;
            this.productService = productService;
            var node = new Uri(configuration.GetConnectionString("ElasticSearchNode"));
            settings = CreateConnectionSettings(node);
            client = CreateElasticClient();
        }

        public void Initialize()
        {
            var products = productService.GetProducts();
            client.IndexMany(products, ProductIndexName);
        }

        private static ConnectionSettings CreateConnectionSettings(Uri node)
        {
            var settings = new ConnectionSettings(node)
                .DefaultIndex(ProductIndexName)
                .DefaultMappingFor<Product>(d => d
                    .IndexName(ProductIndexName)
                );

            return settings;
        }

        public ElasticClient CreateElasticClient()
        {
            var client = new ElasticClient(settings);
            if ((client.Indices.Exists(ProductIndexName)).Exists)
            {
                client.Indices.Delete(ProductIndexName);
            }

            client.Indices.Create(ProductIndexName
                , descriptor => descriptor
                .Map<Product>(m => m
                    .AutoMap()
                    .Properties(ps => ps
                        .Text(s => s
                            .Name(n => n.Code)
                            .Analyzer("substring_analyzer")
                        )
                    )
                )
                .Settings(s => s
                    .Analysis(a => a
                        .Analyzers(analyzer => analyzer
                            .Custom("substring_analyzer", analyzerDescriptor => analyzerDescriptor
                                .Tokenizer("standard")
                                .Filters("lowercase", "substring")
                            )
                        )
                        .TokenFilters(tf => tf
                            .NGram("substring", filterDescriptor => filterDescriptor
                                .MinGram(2)
                                .MaxGram(15)
                            )
                        )
                    )
                )
            );
            return client;
        }

        public async Task<Product> GetProductAsync(string code)
        {
            // returns an IGetResponse mapped 1-to-1 with the Elasticsearch JSON response
            var response =
                await client
                        .GetAsync<Product>(code,
                            idx => idx.Index(ProductIndexName));
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

            return new SearchProductsViewModel(products, searchText);
        }
    }
}
