using Microsoft.AspNetCore.Mvc;
using MVC.Areas.Catalog.Models;
using MVC.Areas.Catalog.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MVC.Areas.Catalog.ViewComponents
{
    public class CarouselViewComponent : ViewComponent
    {
        public CarouselViewComponent()
        {

        }

        public IViewComponentResult Invoke(Category category, List<Product> products, int pageSize)
        {
            var productsInCategory = products
                .Where(p => p.Category.Id == category.Id)
                .ToList();
            int pageCount = (int)Math.Ceiling((double)productsInCategory.Count() / pageSize);

            return View("Default", 
                new CarouselViewModel(category, productsInCategory, pageCount, pageSize));
        }
    }
}
