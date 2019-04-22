using Microsoft.AspNetCore.Mvc;
using MVC.Models.ViewModels;
using System.Collections.Generic;

namespace MVC.ViewComponents
{
    public class BasketListViewComponent : ViewComponent
    {
        public BasketListViewComponent()
        {
        }

        public IViewComponentResult Invoke(List<BasketItem> items, bool isSummary)
        {
            if (items.Count == 0)
            {
                return View("Empty");
            }
            return View("Default", new BasketItemList
            {
                List = items,
                IsSummary = isSummary
            });
        }
    }
}
