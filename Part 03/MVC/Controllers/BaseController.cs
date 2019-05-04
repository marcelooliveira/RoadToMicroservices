﻿using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using System.Diagnostics;
using System.Linq;

namespace MVC.Controllers
{
    public abstract class BaseController : Controller
    {
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        protected string GetUserId()
        {
            return @User.FindFirst(JwtClaimTypes.Subject)?.Value;
        }

        protected string GetUserEmail()
        {
            return @User.FindFirst(JwtClaimTypes.Name)?.Value;
        }
    }

}
