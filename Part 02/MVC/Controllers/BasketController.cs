using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class BasketController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
