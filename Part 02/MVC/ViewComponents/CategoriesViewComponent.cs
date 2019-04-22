using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using System.Collections.Generic;

namespace MVC.ViewComponents
{
    public class CategoriesViewComponent : ViewComponent
    {
        public CategoriesViewComponent()
        {
        }

        public IViewComponentResult Invoke(List<Product> products)
        {
            return View("Default", products);
        }
    }
}
