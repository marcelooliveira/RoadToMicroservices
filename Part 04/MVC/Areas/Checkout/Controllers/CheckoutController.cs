using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC.Areas.Identity.Data;
using MVC.Controllers;
using MVC.Models.ViewModels;
using System.Threading.Tasks;

namespace MVC.Areas.Checkout.Controllers
{
    [Authorize]
    [Area("Checkout")]
    public class CheckoutController : BaseController
    {
        private readonly UserManager<AppIdentityUser> userManager;

        public CheckoutController(UserManager<AppIdentityUser> userManager)
        {
            this.userManager = userManager;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(RegistrationViewModel registration)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(this.User);

                user.Email = registration.Email;
                user.Phone = registration.Phone;
                user.Name = registration.Name;
                user.Address = registration.Address;
                user.AdditionalAddress = registration.AdditionalAddress;
                user.District = registration.District;
                user.City = registration.City;
                user.State = registration.State;
                user.ZipCode = registration.ZipCode;

                await userManager.UpdateAsync(user);
                return View(registration);
            }
            return RedirectToAction("Index", "Registration");
        }
    }
}
