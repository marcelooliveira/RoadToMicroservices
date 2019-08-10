using Microsoft.AspNetCore.Mvc;
using MVC.Areas.Catalog.Models;

namespace MVC.Areas.Catalog.ViewComponents
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
