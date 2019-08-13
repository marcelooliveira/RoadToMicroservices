using API.Catalog.Data;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MVC.Areas.Catalog.Models;
using MVC.Areas.Catalog.Models.ViewModels;
using MVC.Controllers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MVC.Areas.Catalog.Controllers
{
    [Area("Catalog")]
    public class ProductController : BaseController
    {
        private readonly IMapper mapper;
        private readonly IProductService productService;

        public ProductController(IMapper mapper, IProductService productService)
        {
            this.mapper = mapper;
            this.productService = productService;
        }

        public async Task<IActionResult> Index(string searchText)
        {
            var products = await productService.SearchProductsAsync(searchText);

            var viewModel = new SearchProductsViewModel
            {
                Products = mapper.Map<List<Product>>(products),
                SearchText = searchText
            };
            return base.View(viewModel);
        }
    }
}
