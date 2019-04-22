using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.ViewComponents
{
    public class ProductCardViewComponent : ViewComponent
    {
        public ProductCardViewComponent()
        {

        }

        public IViewComponentResult Invoke(Product product)
        {
            return View("Default", product);
        }
    }
}
