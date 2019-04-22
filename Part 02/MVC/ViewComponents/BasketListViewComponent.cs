using Microsoft.AspNetCore.Mvc;
using MVC.Controllers;
using System.Collections.Generic;

namespace MVC.ViewComponents
{
    public class BasketListViewComponent : ViewComponent
    {
        public BasketListViewComponent()
        {
        }

        public IViewComponentResult Invoke(List<BasketItem> items)
        {
            return View("Default", items);
        }
    }
}
