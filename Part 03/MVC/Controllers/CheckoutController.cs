using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC.Areas.Identity.Data;
using MVC.Models.ViewModels;
using System.Threading.Tasks;

namespace MVC.Controllers
{
    [Authorize]
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
                var usuario = await userManager.GetUserAsync(this.User);

                usuario.Email = registration.Email;
                usuario.Phone = registration.Phone;
                usuario.Name = registration.Name;
                usuario.Address = registration.Address;
                usuario.AdditionalAddress = registration.AdditionalAddress;
                usuario.District = registration.District;
                usuario.City = registration.City;
                usuario.State = registration.State;
                usuario.ZipCode = registration.ZipCode;

                await userManager.UpdateAsync(usuario);
                return View(registration);
            }
            return RedirectToAction("Index", "Registration");
        }
    }
}
