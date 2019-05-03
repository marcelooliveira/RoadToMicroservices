using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class CheckoutController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
