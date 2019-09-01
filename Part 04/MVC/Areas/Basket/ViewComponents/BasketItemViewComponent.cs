using Microsoft.AspNetCore.Mvc;
using MVC.Areas.Basket.Model;

namespace MVC.Areas.Basket.ViewComponents
{
    public class BasketItemViewComponent : ViewComponent
    {
        public BasketItemViewComponent()
        {
        }

        public IViewComponentResult Invoke(BasketItem item, bool isSummary = false)
        {
            if (isSummary == true)
            {
                return View("SummaryItem", item);
            }
            return View("Default", item);
        }
    }
}
