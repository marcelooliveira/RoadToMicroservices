using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVC.Areas.Identity.Data;
using MVC.Models.ViewModels;
using MVC.Services;

namespace MVC.Controllers
{
    [Authorize]
    public class RegistrationController : BaseController
    {
        private readonly IHttpHelper httpHelper;

        public RegistrationController(IHttpHelper httpHelper)
        {
            this.httpHelper = httpHelper;
        }

        public IActionResult Index()
        {
            var viewModel = httpHelper.GetRegistration(GetUserId(), GetUserEmail());
            return View(viewModel);
        }
    }
}
