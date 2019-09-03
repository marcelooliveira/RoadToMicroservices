using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC.Areas.Checkout.Model.ViewModels;
using MVC.Areas.Identity.Data;
using MVC.Controllers;
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
        public async Task<IActionResult> Index(CheckoutViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(this.User);

                user.Email = viewModel.Email;
                user.Phone = viewModel.Phone;
                user.Name = viewModel.Name;
                user.Address = viewModel.Address;
                user.AdditionalAddress = viewModel.AdditionalAddress;
                user.District = viewModel.District;
                user.City = viewModel.City;
                user.State = viewModel.State;
                user.ZipCode = viewModel.ZipCode;

                await userManager.UpdateAsync(user);
                               
                return View(viewModel);
            }
            return RedirectToAction("Index", "Registration");
        }
    }
}
