using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace MVC.Areas.Catalog.ViewComponents
{
    public class CategoriesViewComponent : ViewComponent
    {
        const int PageSize = 4;
        public CategoriesViewComponent()
        {
        }

        public IViewComponentResult Invoke(List<Product> products)
        {
            var categories = products
                .Select(p => p.Category)
                .Distinct()
                .ToList();
            return View("Default", new CategoriesViewModel(categories, products, PageSize));
        }
    }
}
