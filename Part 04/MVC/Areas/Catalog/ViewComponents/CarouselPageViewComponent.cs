using Microsoft.AspNetCore.Mvc;
using MVC.Areas.Catalog.Models.ViewModels;
using MVC.Models;
using System.Collections.Generic;
using System.Linq;

namespace MVC.Areas.Catalog.ViewComponents
{
    public class CarouselPageViewComponent : ViewComponent
    {
        public CarouselPageViewComponent()
        {

        }

        public IViewComponentResult Invoke(List<Product> productsInCategory, int pageIndex, int pageSize)
        {
            var productsInPage =
                productsInCategory
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToList();

            return View("Default", 
                new CarouselPageViewModel(productsInPage, pageIndex));
        }
    }
}
