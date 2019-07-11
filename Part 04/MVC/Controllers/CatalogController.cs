using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MVC.Controllers
{
    public class CatalogController : BaseController
    {
        public async Task<IActionResult> Index()
        {
            var products = await SeedData.GetProducts();
            return View(products);
        }
    }
}
