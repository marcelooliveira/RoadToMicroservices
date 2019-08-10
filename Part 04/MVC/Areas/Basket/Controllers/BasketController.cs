using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Controllers;

namespace MVC.Areas.Basket.Controllers
{
    [Authorize]
    [Area("Basket")]
    public class BasketController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
