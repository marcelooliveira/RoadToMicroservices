using Microsoft.AspNetCore.Mvc.ViewComponents;
using MVC.Areas.Basket.Model;
using MVC.Areas.Basket.ViewComponents;
using Xunit;

namespace MVC.Test.ViewComponents
{
    public class BasketItemViewComponentTest
    {
        [Fact]
        public void Invoke_Should_Display_Default_View()
        {
            //arrange 
            var vc = new BasketItemViewComponent();
            BasketItem item =
                new BasketItem { Id = "1", ProductId = "1", ProductName = "Broccoli", UnitPrice = 59.90m, Quantity = 2 };

            //act 
            var result = vc.Invoke(item);

            //assert
            ViewViewComponentResult vvcResult = Assert.IsAssignableFrom<ViewViewComponentResult>(result);
            Assert.Equal("Default", vvcResult.ViewName);
            BasketItem resultModel = Assert.IsAssignableFrom<BasketItem>(vvcResult.ViewData.Model);
            Assert.Equal(item.ProductId, resultModel.ProductId);
        }

        [Fact]
        public void Invoke_Should_Display_SummaryItem_View()
        {
            //arrange 
            var vc = new BasketItemViewComponent();
            BasketItem item =
                new BasketItem { Id = "2", ProductId = "5", ProductName = "Green Grapes", UnitPrice = 59.90m, Quantity = 3 };

            //act 
            var result = vc.Invoke(item, true);

            //assert
            ViewViewComponentResult vvcResult = Assert.IsAssignableFrom<ViewViewComponentResult>(result);
            Assert.Equal("SummaryItem", vvcResult.ViewName);
            BasketItem resultModel = Assert.IsAssignableFrom<BasketItem>(vvcResult.ViewData.Model);
            Assert.Equal(item.ProductId, resultModel.ProductId);
        }
    }
}
