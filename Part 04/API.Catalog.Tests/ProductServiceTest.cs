using API.Catalog.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace API.Catalog.Tests
{
    public class ProductServiceTest
    {
        [Fact]
        public async Task GetProductsAsync_Should_Return_30_ProductsAsync()
        {
            //arrange
            IProductService productService = new ProductService();

            //act
            List<Models.Product> products = await productService.GetProductsAsync();

            //assert
            Assert.Equal(30, products.Count);
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        public async Task SearchProductsAsync_Should_Return_30_ProductsAsync(string searchText)
        {
            //arrange
            IProductService productService = new ProductService();

            //act
            List<Models.Product> products = await productService.SearchProductsAsync(searchText);

            //assert
            Assert.Equal(30, products.Count);
        }

        [Theory]
        [InlineData("Grapes", "Grapes")]
        [InlineData("Oranges", "Oranges")]
        [InlineData("ORANGES", "Oranges")]
        [InlineData("oranges", "Oranges")]
        [InlineData("oranges ", "Oranges")]
        public async Task SearchProductsAsync_Should_Return_OneProduct(string searchText, string expected)
        {
            //arrange
            IProductService productService = new ProductService();

            //act
            List<Models.Product> products = await productService.SearchProductsAsync(searchText);
                
            //assert
            Assert.Collection(products, (p) =>
                {
                    Assert.Equal(expected, p.Name);
                });
        }

        [Fact]
        public async Task SearchProductsAsync_Should_Return_NoProducts()
        {
            //arrange
            IProductService productService = new ProductService();

            //act
            List<Models.Product> products = await productService.SearchProductsAsync("OrangesXPTO");

            //assert
            Assert.Empty(products);
        }
    }
}
