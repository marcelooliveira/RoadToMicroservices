using Microsoft.AspNetCore.Mvc;
using MVC.Controllers;
using System.Threading.Tasks;

namespace MVC.Areas.Catalog.Controllers
{
    [Area("Catalog")]
    public class ProductController : BaseController
    {
        public async Task<IActionResult> Index()
        {
            var products = await SeedData.GetProducts();
            return View(products);
        }
    }
}
