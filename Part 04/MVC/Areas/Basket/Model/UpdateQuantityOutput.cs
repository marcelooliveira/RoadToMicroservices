﻿namespace MVC.Areas.Basket.Model
{
    public class UpdateQuantityOutput
    {
        public UpdateQuantityOutput(BasketItem basketItem, CustomerBasket customerBasket)
        {
            BasketItem = basketItem;
            CustomerBasket = customerBasket;
        }

        public BasketItem BasketItem { get; }
        public CustomerBasket CustomerBasket { get; }
    }
}
