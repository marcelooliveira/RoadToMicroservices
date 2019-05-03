using System.Collections.Generic;
using MVC.Models.ViewModels;

namespace MVC.Services
{
    public interface IBasketService
    {
        List<BasketItem> GetBasketItems();
    }
}