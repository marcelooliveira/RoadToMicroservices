using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Controllers;

namespace MVC.Areas.Notification.Controllers
{
    [Authorize]
    [Area("Notification")]
    public class NotificationController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
