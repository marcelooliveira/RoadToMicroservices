using System.Collections.Generic;
using MVC.Models.ViewModels;

namespace MVC.Areas.Basket.Services
{
    public interface IBasketService
    {
        List<BasketItem> GetBasketItems();
    }
}