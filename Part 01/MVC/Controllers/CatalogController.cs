using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using System.Diagnostics;

namespace MVC.Controllers
{
    public class CatalogController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
