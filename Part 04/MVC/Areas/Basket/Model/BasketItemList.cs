using System.Collections.Generic;

namespace MVC.Areas.Basket.Model
{
    public class BasketItemList
    {
        public List<BasketItem> List { get; set; }
        public bool IsSummary { get; set; }
    }
}
