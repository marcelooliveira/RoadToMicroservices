using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC.Areas.Basket.Data;
using MVC.Areas.Basket.Model;
using MVC.Areas.Checkout.Data;
using MVC.Areas.Checkout.Model;
using MVC.Areas.Checkout.Model.ViewModels;
using MVC.Areas.Identity.Data;
using MVC.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Areas.Checkout.Controllers
{
    [Authorize]
    [Area("Checkout")]
    public class CheckoutController : BaseController
    {
        private readonly IBasketRepository basketRepository;
        private readonly ICheckoutRepository checkoutRepository;
        private readonly UserManager<AppIdentityUser> userManager;

        public CheckoutController(
            IBasketRepository basketRepository,
            ICheckoutRepository checkoutRepository,
            UserManager<AppIdentityUser> userManager)
        {
            this.basketRepository = basketRepository;
            this.checkoutRepository = checkoutRepository;
            this.userManager = userManager;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(CheckoutViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(this.User);

                await UpdateUser(viewModel, user);

                await CreateOrder(user);

                return View(viewModel);
            }
            return RedirectToAction("Index", "Registration");
        }

        private async Task CreateOrder(AppIdentityUser user)
        {
            var customerBasket = await basketRepository.GetBasketAsync(user.Id);

            var items = customerBasket.Items
                .Select(i =>
                    new OrderItem(i.ProductId, i.ProductName, i.UnitPrice, i.Quantity))
                .ToList();

            var order = new Order(items, user.Id, user.Name, user.Email, user.Phone, user.Address, user.AdditionalAddress, 
                user.District, user.City, user.State, user.ZipCode);
            await checkoutRepository.CreateOrUpdate(order);
        }

        private async Task UpdateUser(CheckoutViewModel viewModel, AppIdentityUser user)
        {
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
        }
    }
}
