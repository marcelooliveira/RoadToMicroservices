using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    [Authorize]
    public class NotificationController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
