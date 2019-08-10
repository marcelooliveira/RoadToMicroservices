using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC.Areas.Identity.Data;
using MVC.Areas.Registration.Models.ViewModels;
using MVC.Controllers;
using MVC.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Areas.Registration.Controllers
{
    [Authorize]
    [Area("Registration")]
    public class RegistrationController : BaseController
    {
        private readonly UserManager<AppIdentityUser> userManager;

        public RegistrationController(UserManager<AppIdentityUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await userManager.GetUserAsync(this.User);
            var viewModel = new RegistrationViewModel(
                user.Id, user.Name, user.Email, user.Phone,
                user.Address, user.AdditionalAddress, user.District,
                user.City, user.State, user.ZipCode
            );
            return View(viewModel);
        }
    }
}
