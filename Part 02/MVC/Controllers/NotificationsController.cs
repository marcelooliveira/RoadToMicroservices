using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class NotificationsController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
