using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Areas.Identity.Data;
using MVC.Models.ViewModels;
using MVC.Services;

namespace MVC.Controllers
{
    [Authorize]
    public class RegistrationController : BaseController
    {
        private readonly IIdentityParser<AppIdentityUser> appUserParser;

        public RegistrationController(IIdentityParser<AppIdentityUser> appUserParser)
        {
            this.appUserParser = appUserParser;
        }

        public IActionResult Index()
        {
            var usuario = appUserParser.Parse(HttpContext.User);
            var viewModel = new RegistrationViewModel
            {
                District = usuario.District,
                ZipCode = usuario.ZipCode,
                AdditionalAddress = usuario.AdditionalAddress,
                Email = usuario.Email,
                Address = usuario.Address,
                City = usuario.City,
                Name = usuario.Name,
                Phone = usuario.Phone,
                State = usuario.State
            };
            return View(viewModel);
        }
    }
}
