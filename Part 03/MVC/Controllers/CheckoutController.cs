using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Models.ViewModels;
using System.Threading.Tasks;

namespace MVC.Controllers
{
    [Authorize]
    public class CheckoutController : BaseController
    {
        private readonly IHttpHelper httpHelper;

        public CheckoutController(IHttpHelper httpHelper)
        {
            this.httpHelper = httpHelper;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(RegistrationViewModel registration)
        {
            if (ModelState.IsValid)
            {
                httpHelper.SetRegistration(GetUserId(), registration);
                return View(registration);
            }
            return RedirectToAction("Index", "Registration");
        }
    }
}
