﻿using MVC.Areas.Basket.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MVC.Areas.Basket.Data
{
    public interface IBasketRepository
    {
        Task<CustomerBasket> GetBasketAsync(string customerId);
        IEnumerable<string> GetUsers();
        Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket);
        Task<CustomerBasket> AddBasketAsync(string customerId, BasketItem item);
        Task<UpdateQuantityOutput> UpdateBasketAsync(string customerId, UpdateQuantityInput item);
        Task<bool> DeleteBasketAsync(string id);
    }
}