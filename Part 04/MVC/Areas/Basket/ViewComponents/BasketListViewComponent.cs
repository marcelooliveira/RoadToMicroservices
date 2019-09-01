using Microsoft.AspNetCore.Mvc;
using MVC.Areas.Basket.Model;
using System.Collections.Generic;

namespace MVC.Areas.Basket.ViewComponents
{
    public class BasketListViewComponent : ViewComponent
    {
        public BasketListViewComponent()
        {
        }

        public IViewComponentResult Invoke(CustomerBasket customerBasket, bool isSummary = false)
        {
            if (customerBasket.Items.Count == 0)
            {
                return View("Empty");
            }
            return View("Default", new BasketItemList
            {
                List = customerBasket.Items,
                IsSummary = isSummary
            });
        }
    }
}
