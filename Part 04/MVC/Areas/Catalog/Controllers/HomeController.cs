using Microsoft.AspNetCore.Mvc;
using MVC.Areas.Catalog.Data;
using MVC.Areas.Catalog.Data.Repositories;
using MVC.Controllers;
using System.Threading.Tasks;

namespace MVC.Areas.Catalog.Controllers
{
    [Area("Catalog")]
    public class HomeController : BaseController
    {
        private readonly IProductRepository productRepository;

        public HomeController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public async Task<IActionResult> Index(string searchText)
        {
            var viewModel = await productRepository.GetProductsAsync(searchText);
            return base.View(viewModel);
        }
    }
}
