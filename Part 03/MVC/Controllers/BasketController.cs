using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    [Authorize]
    public class BasketController : BaseController
    {
        public IActionResult Index()
        {
            var clientId = GetUserId();

            return View();
        }
    }
}
