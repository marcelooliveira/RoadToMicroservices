using Microsoft.AspNetCore.Mvc.ViewComponents;
using MVC.Areas.Basket.Model;
using MVC.Areas.Basket.ViewComponents;
using System.Collections.Generic;
using Xunit;

namespace MVC.Test.ViewComponents
{
    public class BasketListViewComponentTest
    {
        [Fact]
        public void Invoke_With_Items_Should_Display_Default_View()
        {
            //arrange
            List<BasketItem> items =
            new List<BasketItem>
            {
                new BasketItem { Id = "1", ProductId = "1", ProductName = "Broccoli", UnitPrice = 59.90m, Quantity = 2 },
                new BasketItem { Id = "2", ProductId = "5", ProductName = "Green Grapes", UnitPrice = 59.90m, Quantity = 3 },
                new BasketItem { Id = "3", ProductId = "9", ProductName = "Tomato", UnitPrice = 59.90m, Quantity = 4 }
            };
            CustomerBasket customerBasket =  new CustomerBasket { Items = items };

            var vc = new BasketListViewComponent();

            //act 
            var result = vc.Invoke(customerBasket, false);

            //assert
            ViewViewComponentResult vvcResult = Assert.IsAssignableFrom<ViewViewComponentResult>(result);
            Assert.Equal("Default", vvcResult.ViewName);
            var model = Assert.IsAssignableFrom<BasketItemList>(vvcResult.ViewData.Model);
            Assert.Collection<BasketItem>(model.List,
                i => Assert.Equal("1", i.ProductId),
                i => Assert.Equal("5", i.ProductId),
                i => Assert.Equal("9", i.ProductId)
            );
        }

        [Fact]
        public void Invoke_Without_Items_Should_Display_Empty_View()
        {
            //arrange 
            var items = new List<BasketItem>();
            CustomerBasket customerBasket = new CustomerBasket { Items = items };
            var vc = new BasketListViewComponent();

            //act 
            var result = vc.Invoke(customerBasket, false);

            //assert
            ViewViewComponentResult vvcResult = Assert.IsAssignableFrom<ViewViewComponentResult>(result);
            Assert.Equal("Empty", vvcResult.ViewName);
        }
    }
}
