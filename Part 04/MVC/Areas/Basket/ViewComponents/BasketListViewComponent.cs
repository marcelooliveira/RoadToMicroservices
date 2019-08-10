using Microsoft.AspNetCore.Mvc;
using MVC.Models.ViewModels;
using MVC.Areas.Basket.Services;
using System.Collections.Generic;

namespace MVC.Areas.Basket.ViewComponents
{
    public class BasketListViewComponent : ViewComponent
    {
        private readonly IBasketService basketService;

        public BasketListViewComponent(IBasketService basketService)
        {
            this.basketService = basketService;
        }

        public IViewComponentResult Invoke(bool isSummary = false)
        {
            List<BasketItem> items = basketService.GetBasketItems();

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
