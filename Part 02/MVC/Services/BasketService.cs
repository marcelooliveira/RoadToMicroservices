using MVC.Models.ViewModels;
using System.Collections.Generic;

namespace MVC.Services
{
    public class BasketService : IBasketService
    {
        public List<BasketItem> GetBasketItems()
        {
            return new List<BasketItem>
            {
                new BasketItem { Id = 1, ProductId = 1, Name = "Broccoli", UnitPrice = 59.90m, Quantity = 2 },
                new BasketItem { Id = 2, ProductId = 5, Name = "Green Grapes", UnitPrice = 59.90m, Quantity = 3 },
                new BasketItem { Id = 3, ProductId = 9, Name = "Tomato", UnitPrice = 59.90m, Quantity = 4 }
            };
        }
    }
}
