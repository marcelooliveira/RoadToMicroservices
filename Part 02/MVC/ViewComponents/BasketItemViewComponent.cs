using Microsoft.AspNetCore.Mvc;
using MVC.Models.ViewModels;

namespace MVC.ViewComponents
{
    public class BasketItemViewComponent : ViewComponent
    {
        public BasketItemViewComponent()
        {
        }

        public IViewComponentResult Invoke(BasketItem item)
        {
            return View("Default", item);
        }
    }
}
