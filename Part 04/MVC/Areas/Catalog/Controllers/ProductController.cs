using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MVC.Areas.Catalog.Models;
using MVC.Controllers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MVC.Areas.Catalog.Controllers
{
    [Area("Catalog")]
    public class ProductController : BaseController
    {
        private readonly IMapper mapper;

        public ProductController(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var products = await ProductSeedData.GetProducts();
            List<Product> model = mapper.Map<List<Product>>(products);
            return base.View(model);
        }
    }
}
